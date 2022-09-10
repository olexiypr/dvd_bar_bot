using DvdBarBot.Sender;
using Telegram.Bot.Types;
using static DvdBarBot.Sender.CatalogSender;
using User = DvdBarBot.Entities.User;

namespace DvdBarBot.States.CatalogStates;

public class CatalogState : State
{
    private delegate Task OnCreating(User user);
    private List<Message> SentMessages { get; set; }
    public CatalogState(User user)
    {
        OnCreating onCreating = SendCategoriesAsync;
        onCreating.Invoke(user);
    }
    public override async void ChangeState(User user, Update update)
    {
        if (update.CallbackQuery is not { } callbackQuery)
            return;
        var categoryId = int.Parse(callbackQuery.Data?.Split("_")[1]);
        if (SentMessages != null)
        {
            DeleteMessagesWithProductAnotherCategory(user, SentMessages);
        }
        SentMessages = await SendAllProductsInCategoryAsync(user, categoryId);
    }
}