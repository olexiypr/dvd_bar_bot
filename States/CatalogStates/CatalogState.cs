using DvdBarBot.Sender;
using Telegram.Bot.Types;
using static System.Int32;
using static DvdBarBot.Sender.CatalogSender;
using User = DvdBarBot.Entities.User;

namespace DvdBarBot.States.CatalogStates;
/// <summary>
/// The state the user enters when he selects a directory from the Menu.Menu
/// </summary>
public class CatalogState : State
{
    private delegate Task OnCreating(User user);
    private List<Message>? SentMessages { get; set; }
    public CatalogState(User user)
    {
        OnCreating onCreating = SendCategoriesAsync;
        onCreating.Invoke(user);
    }
    /// <summary>
    /// Method to send products from a category and delete sent messages when another category is selected
    /// </summary>
    /// <param name="user"></param>
    /// <param name="update"></param>
    public override async void ChangeState(User user, Update update)
    {
        if (update.CallbackQuery is not { } callbackQuery)
            return;
        if (TryParse(callbackQuery.Data?.Split("_")[1], out var categoryId))
        {
            if (SentMessages != null)
            {
                await DeleteMessagesWithProductAnotherCategory(user, SentMessages);
            }
            SentMessages = await SendAllProductsInCategoryAsync(user, categoryId);
        }
    }
}