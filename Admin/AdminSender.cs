using System.Data.Entity;
using System.Text;
using DvdBarBot.DataBase;
using DvdBarBot.Entities;
using DvdBarBot.Interfaces;
using DvdBarBot.Menu;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace DvdBarBot.Admin;

public static class AdminSender
{
    private static IChatId _admin;

    private static IChatId Admin => DvdBarBot.Admin.Admin.Instance;

    public static ITelegramBotClient botClient { get; set; }
    public static CancellationToken cancellationToken { get; set; }
    public static IGetAllProducts GetAllProducts { get; set; }
    public static async Task SendMenuAsync()
    {
        await botClient.SendTextMessageAsync(chatId: Admin.ChatId,
            text: "Меню",
            replyMarkup: AdminMenu.GetMenu,
            cancellationToken: cancellationToken);
    }

    public static async Task<List<Message>> SendAllProductsAsync()
    {
        var sentMessages = new List<Message>();
        var allProducts = GetAllProducts.GetAllProductsAsync().Result;
        if (!allProducts.Any())
        {
            sentMessages.Add(await botClient.SendTextMessageAsync(chatId: Admin.ChatId,
                text: "Всі товари видалені!",
                cancellationToken: cancellationToken));
             return sentMessages;
        }
        foreach (var product in allProducts)
        {
            InlineKeyboardMarkup keyboardMarkup = new(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Видалити", $"del_product_{product.Id}"),
                }
            });
            sentMessages.Add(await botClient.SendTextMessageAsync(chatId: Admin.ChatId,
                text: product.ToString(),
                cancellationToken: cancellationToken,
                replyMarkup: keyboardMarkup));
        }

        InlineKeyboardMarkup saveChangesMarcup = new(InlineKeyboardButton.WithCallbackData("От сюди", "save_changes"));
        sentMessages.Add(await botClient.SendTextMessageAsync(chatId: Admin.ChatId,
            cancellationToken: cancellationToken,
            replyMarkup: saveChangesMarcup,
            text: "Для збереження змін натисни сюди"));

        return sentMessages;
    }

    public static async Task<Message> ChangeMessageAsync(Message message)
    {
        var productId = message.ReplyMarkup.InlineKeyboard.First().First().CallbackData.Split("_")[2];
        bool isChanged = message.Text.Contains("Видалено");
        InlineKeyboardMarkup keyboardMarkup = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Повернути", $"ret_product_{productId}"),
            }
        });
        InlineKeyboardMarkup returnMarkup = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Видалити", $"del_product_{productId}"),
            }
        });
        var messageText = GetBaseMessage(message.Text);
        return await botClient.EditMessageTextAsync(messageId: message.MessageId,
            cancellationToken: cancellationToken,
            text: !isChanged ? $"{messageText}\nВидалено!✅" : $"{messageText}\n✅Повернуто!",
            replyMarkup: !isChanged ? keyboardMarkup : returnMarkup,
            chatId: message.Chat.Id);
    }

    private static string GetBaseMessage(string? messageText)
    {
        var lines = messageText.Split("\n");
        return $"{lines[0]}\n{lines[1]}\n{lines[2]}";
    }

    public static async Task DeleteMessagesAsync(IEnumerable<Message> messages)
    {
        foreach (var message in messages)
        {
            await botClient.DeleteMessageAsync(messageId: message.MessageId,
                chatId: message.Chat.Id,
                cancellationToken: cancellationToken);
        }
    }

    public static async Task SentSubmitToAddProducts()
    {
        await botClient.SendTextMessageAsync(chatId: Admin.ChatId,
            text: "Для додавання товару введіть:\n" +
                  "Назву, ціну, опис в форматі:\n" +
                  "назва\n" +
                  "ціна\n" +
                  "опис\n",
            cancellationToken: cancellationToken); 
    }

    public static async Task SendInvalidDataWarning()
    {
        await botClient.SendTextMessageAsync(chatId: Admin.ChatId,
            text: "Неправильний формат!",
            cancellationToken: cancellationToken);
    }

    public static async Task<Message> SendAddedProductInfoAsync(Product product)
    {
        var productCategories = new ApplicationDbContext().ProductCategories;
        InlineKeyboardMarkup keyboardMarkup = new(new[]
        {
            productCategories
                .Select(category =>
                    InlineKeyboardButton.WithCallbackData($"{category.Name}", $"category_{category.Id}")).ToArray()
        });
        var text = $"Виберіть категорію для товару\nЗараз він виглядає так:\n--------------------\n" + product.ToString();
        return await botClient.SendTextMessageAsync(chatId: Admin.ChatId,
            text: text,
            cancellationToken: cancellationToken,
            replyMarkup: keyboardMarkup);
    }

    public static async Task ChangeMessageForSubmitAddProduct(Message message)
    {
        await botClient.EditMessageTextAsync(chatId: message.Chat.Id,
            messageId: message.MessageId,
            cancellationToken: cancellationToken,
            replyMarkup: InlineKeyboardMarkup.Empty(),
            text: "Додано✅");
    }

    public static async Task SendSubmitToSaveChangesInDb()
    {
        await botClient.SendTextMessageAsync(chatId: Admin.ChatId,
            text: "Зміни збережено!✅",
            cancellationToken: cancellationToken);
    }

    public static async Task SendCountUsersAsync()
    {
        await using var dbContext = new ApplicationDbContext();
        var count = dbContext.users.Count();
        await botClient.SendTextMessageAsync(chatId: Admin.ChatId,
            text: $"Загальна кількість користувачів: {count}",
            cancellationToken: cancellationToken);
    }
}