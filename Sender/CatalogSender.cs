using DvdBarBot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using static DvdBarBot.Sender.Sender;
using User = DvdBarBot.Entities.User;

namespace DvdBarBot.Sender;

public static class CatalogSender
{
    public static readonly IGetAllProductsInCategory AllProductsInCategory = Sender.dbContext;
    public static readonly IGetAllCategories AllCategories = Sender.dbContext;
    public static async Task SendCategoriesAsync(User user)
    {
        foreach (var category in AllCategories.ProductCategories)
        {
            InlineKeyboardMarkup keyboardMarkup = new(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Вибрати", $"category_{category.Id}"),
                }
            });
            var text = $"{category.Name}";
            await Sender.botClient.SendTextMessageAsync(chatId: user.ChatId,
                replyMarkup: keyboardMarkup,
                text: text,
                cancellationToken: cancellationToken);
        }
    }

    public static async Task<List<Message>> SendAllProductsInCategoryAsync(User user, int categoryId)
    {
        var sentMessages = new List<Message>();
        foreach (var product in await AllProductsInCategory.GetProductsInCategoryAsync(categoryId))
        {
            InlineKeyboardMarkup keyboardMarkup = new(new[]
            {
                InlineKeyboardButton.WithUrl("Зв'язатися з менеджером @olexiypr для покупки", "https://t.me/olexiypr"),
            });
            sentMessages.Add(await Sender.botClient.SendTextMessageAsync(chatId: user.ChatId,
                text: product.GetMessageString(),
                replyMarkup: keyboardMarkup,
                cancellationToken: cancellationToken));
        }

        return sentMessages;
    }

    public static Task DeleteMessagesWithProductAnotherCategory(User user, List<Message> messages)
    {
        async void Action(Message message) => 
            await Sender.botClient.DeleteMessageAsync(chatId: user.ChatId,
                messageId: message.MessageId, 
                cancellationToken: cancellationToken);
        messages.ForEach(Action);
        return Task.CompletedTask;
    }
}