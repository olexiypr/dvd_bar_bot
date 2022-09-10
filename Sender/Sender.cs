using System.Net.Mime;
using DvdBarBot.DataBase;
using DvdBarBot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = DvdBarBot.Entities.User;

namespace DvdBarBot.Sender;

public static class Sender
{
    public static ITelegramBotClient botClient { get; set; }
    public static CancellationToken cancellationToken { get; set; }
    public static ApplicationDbContext dbContext { get; set; }
    
    public static async Task PropositionSubscribeChannelAsync(User user)
    {
        await botClient.SendTextMessageAsync(chatId: user.ChatId,
            text: "Для використання боту підпишіться на канал @testMyBotFuctions\n" +
                  "Та натисніть /start",
            cancellationToken: cancellationToken);
    }

    public static async Task SayHalloAsync(User user)
    {
        await botClient.SendTextMessageAsync(chatId: user.ChatId,
            text: $"Привіт, {user.Name}! Вибери що хочеш зробити",
            cancellationToken: cancellationToken);
    }

    public static async Task SendMenuAsync(User user)
    {
        await botClient.SendTextMessageAsync(chatId: user.ChatId,
            text: "Меню",
            replyMarkup: Menu.Menu.GetMenu,
            cancellationToken: cancellationToken);
    }
    public static async Task SendStubAsync(User user)
    {
        await botClient.SendTextMessageAsync(chatId: user.ChatId,
            text: $"{user.Name}, я не розумію твоєї команди",
            cancellationToken: cancellationToken);
    }
}