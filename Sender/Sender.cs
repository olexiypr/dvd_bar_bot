using System.Data.Entity;
using System.Net.Mime;
using DvdBarBot.Admin;
using DvdBarBot.DataBase;
using DvdBarBot.Entities;
using DvdBarBot.Interfaces;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = DvdBarBot.Entities.User;

namespace DvdBarBot.Sender;
/// <summary>
/// A set of static methods for sending messages to the user
/// </summary>
public static class Sender
{
    public static ITelegramBotClient botClient { get; set; }
    public static CancellationToken cancellationToken { get; set; }

    public static ApplicationDbContext dbContext
    {
        get => _dbContext;
        set
        {
            _dbContext = value;
            AdminSender.GetAllProducts = value;
        }
    }
    private static ApplicationDbContext _dbContext;

    public static async Task SendPropositionSubscribeChannelAsync(IChatId user)
    {
        await SendTextMessageAsync(user, "Для використання боту підпишіться на канал @testMyBotFuctions\nТа натисніть /start");
    }

    private static async Task SendTextMessageAsync(IChatId user, string text)
    {
        await botClient.SendTextMessageAsync(chatId: user.ChatId,
            text: text,
            cancellationToken: cancellationToken);
    }
    public static async Task SayHalloAsync(User user)
    {
        await SendTextMessageAsync(user, $"Привіт, {user.Name}! Вибери що хочеш зробити");
    }

    public static async Task SendMenuAsync(IChatId user)
    {
        await botClient.SendTextMessageAsync(chatId: user.ChatId,
            text: "Меню",
            replyMarkup: Menu.Menu.GetMenu,
            cancellationToken: cancellationToken);
    }
    public static async Task SendStubAsync(User user)
    {
        await SendTextMessageAsync(user, $"{user.Name}, я не розумію твоєї команди");
    }

    public static async Task<Message> SendPropositionToRaffleAsync(User user)
    {
        var raffle = Raffle.Instance;
        InlineKeyboardMarkup keyboardMarkup = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData($"Беру участь. Кількість учасників {raffle.CurrentUserCount}/{raffle.MaxUsersCount}", "take_part_raffle"),
                InlineKeyboardButton.WithCallbackData("Не цікаво", "dont_take_part"), 
            }
        });
        return await botClient.SendTextMessageAsync(chatId: user.ChatId,
            text: $"Привіт, {user.Name} тобі випала можливість прийняти участь в розіграші" +
                  $" промокоду на знижку {raffle.Promocode.Discount}%",
            cancellationToken: cancellationToken,
            replyMarkup: keyboardMarkup);
    }
    
    public static async Task SendSubmitToTakePartInRaffle(IChatId user, Message? message)
    {
        InlineKeyboardMarkup keyboardMarkup = new(new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData($"Приймаєш участь!✅", "take_part_raffle"),}
        });
        await botClient.EditMessageTextAsync(chatId: user.ChatId,
            messageId: message.MessageId,
            cancellationToken: cancellationToken,
            replyMarkup: keyboardMarkup,
            text: message.Text);
    }
    public static async Task SendSubmitToDontTakePartInRaffle(IChatId user, Message? message)
    {
        InlineKeyboardMarkup keyboardMarkup = new(new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData($"Якщо передумаєш натисни сюди", "take_part_raffle"),}
        });
        await botClient.EditMessageTextAsync(chatId: user.ChatId,
            messageId: message.MessageId,
            cancellationToken: cancellationToken,
            replyMarkup: keyboardMarkup,
            text: message.Text);
    }

    public static async Task SendInfoForAlreadyParticipatingRaffleAsync(User user)
    {
        await SendTextMessageAsync(user, "Ви вже приймаєте участь в розіграші");
    }
}