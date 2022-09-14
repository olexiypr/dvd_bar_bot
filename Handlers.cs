using DvdBarBot.Interfaces;
using DvdBarBot.States;
using Telegram.Bot;
using static DvdBarBot.Handlers;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = DvdBarBot.Entities.User;
namespace DvdBarBot;

public static class Handlers
{
    private static ITelegramBotClient _botClient;

    private static CancellationToken _cancellationToken;
    public static ITelegramBotClient botClient
    {
        get => _botClient;
        set
        {
            _botClient = value;
            Sender.Sender.botClient = value;
        }
    }

    public static CancellationToken cancellationToken
    {
        get => _cancellationToken;
        set
        {
            _cancellationToken = value;
            Sender.Sender.cancellationToken = value;
        }
    }

    public static IAddUser AddUser { get; set; }

    public static Dictionary<long, User> Users { get; set; }
    public static Admin.Admin Admin { get; set; }
    public static async Task HandleUpdateAsync(Update update)
    {
        if (update.Message is not { } && update.CallbackQuery is not { })
        {
            return;
        }
        var chatId = update.Message == null ? update.CallbackQuery.Message.Chat.Id : update.Message.Chat.Id;
        if (chatId == DvdBarBot.Admin.Admin.ChatId)
        {
            await HandleAdminUpdateAsync(update);
            return;
        }
        switch (update.Type)
        {
            case UpdateType.Message:
            {
                await HandleMessageAsync(update.Message);
                break;
            }
            case UpdateType.CallbackQuery:
            {
                if (!Users.ContainsKey(chatId))
                {
                    var callback = update.CallbackQuery;
                    Users.Add(callback.Message.Chat.Id, new User(callback.Message.Chat.Id, callback.Message.From));
                }
            }
                break;
        }

        var message = update.Message;
        if (message != null && !Users.ContainsKey(message.Chat.Id))
        {
            Users.Add(message.Chat.Id, new User(message.Chat.Id, message.From));
        }
        
        Users[chatId].ProcessUpdate(update);
    }

    private static async Task HandleAdminUpdateAsync(Update update)
    {
        if (Admin == null)
        {
            Admin = new Admin.Admin(update.Message.From);
        }
            
        switch (update.Type)
        {
            case UpdateType.Message:
            {
                Admin.ProcessUpdate(update);
                return; 
            }
            case UpdateType.CallbackQuery:
            {
                Admin.ProcessUpdate(update);
                return;
            }
        }
    }

    public static async Task HandleCallbackQueryAsync(CallbackQuery callbackQuery)
    {
        /*if (!Users.ContainsKey(chatId))
        {
            Users.Add(message.Chat.Id, new User(message.Chat.Id, await IsSubscriberAsync(message), message.From));
        }*/
    }
    public static async Task HandleMessageAsync(Message message)
    {
        if (message.Text == "/restart")
        {
            Users[message.Chat.Id] = new User(message.Chat.Id, message.From);
        }
        var chatId = message.Chat.Id;
        if (!Users.ContainsKey(chatId))
        {
            Users.Add(message.Chat.Id, new User(message.Chat.Id, message.From));
            await AddUser.AddUserAsync(Users[chatId]);
        }
    }
    public static async Task<bool> IsSubscriberAsync(Message message)
    {
        if (message.From != null)
        { 
            var chatMember = await botClient.GetChatMemberAsync(chatId: "@testMyBotFuctions", 
                userId:message.From.Id, 
                cancellationToken: cancellationToken);
            if (chatMember is not ChatMemberLeft)
            {
                return true;
            }
        }

        return false;
    }
    public static async Task<bool> IsSubscriberAsync(User user)
    {
        var chatMember = await botClient.GetChatMemberAsync(chatId: "@testMyBotFuctions", 
            userId:user.UserId, 
            cancellationToken: cancellationToken);
        if (chatMember is not ChatMemberLeft)
        {
            return true;
        }
        return false;
    }
}