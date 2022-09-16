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
            GetRaffle = value;
            AdminSender.GetAllProducts = value;
        }
    }

    public static IGetRaffle GetRaffle { get; set; }
    private static ApplicationDbContext _dbContext;

    public static async Task SendPropositionSubscribeChannelAsync(User user)
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

    public static async Task<Message> SendPropositionToRaffleAsync(User user)
    {
        var raffle = Raffle.Instance;
        InlineKeyboardMarkup keyboardMarkup = new(new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData($"Беру участь. Кількість учасників {raffle.CurrentUserCount}/{raffle.MaxUsersCount}", "take_part_raffle"),}
        });
        return await botClient.SendTextMessageAsync(chatId: user.ChatId,
            text: $"Привіт, {user.Name} тобі випала можливість прийняти участь в розіграші" +
                  $" промокоду на знижку {raffle.Promocode.Discount}%",
            cancellationToken: cancellationToken,
            replyMarkup: keyboardMarkup);
    }

    public static async Task SendSubmitToTakePartInRaffle(User user, Message message)
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

    public static async Task SendInfoForAlreadyParticipatingRaffleAsync(User user)
    {
        await botClient.SendTextMessageAsync(chatId: user.ChatId,
            cancellationToken: cancellationToken,
            text: "Ви вже приймаєте участь в розіграші");
    }
}