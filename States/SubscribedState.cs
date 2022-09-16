using DvdBarBot.States.CatalogStates;
using Telegram.Bot.Types;
using static DvdBarBot.Sender.Sender;
using User = DvdBarBot.Entities.User;

namespace DvdBarBot.States;
/// <summary>
/// User state when he subscribed to the channel (method Handlers.IsSubscribed return true)
/// </summary>
public class SubscribedState : State
{
    private delegate Task OnCreating (User user);

    private int count = 0;
    public SubscribedState(User user)
    {
        OnCreating onCreating = Creating;
        onCreating.Invoke(user);
    }
    public SubscribedState(User user, bool isNeedInvokeCreating)
    {
        if (isNeedInvokeCreating)
        {
            OnCreating onCreating = Creating;
            onCreating.Invoke(user);
        }
    }
    /// <summary>
    /// Method that is invoked to greet the user after he has pressed /start 
    /// </summary>
    /// <param name="user"></param>
    public async Task Creating(User user)
    {
        await SayHalloAsync(user);
        await SendMenuAsync(user);
    }
    /// <summary>
    /// Method that handles user clicks on a menu item (Menu.Menu)
    /// </summary>
    /// <param name="user">User who clicked</param>
    /// <param name="update">Update (It is expected that this is one of the menu items)</param>
    public override async void ChangeState(User user, Update update)
    {
        if (update.CallbackQuery is { } callbackQuery)
        {
            if (callbackQuery.Data.StartsWith("take_part_raffle") && user.Raffle != null)
            {
                count++;
                if (count != 5) return;
                count = 0;
                await SendInfoForAlreadyParticipatingRaffleAsync(user);
            }
            else if (callbackQuery.Data.StartsWith("take_part_raffle") && user.Raffle == null)
            {
                await SendSubmitToTakePartInRaffle(user, user.SentMessage);
            }
            return;
        }
        if (update.Message is not {Text: { } messageText})
            return;
        switch (messageText)
        {
            case "Каталог":
            {
                user.State = new CatalogState(user);
                return;
            }
            case "Пропозиції та акції":
            {
                return;
            }
            case "Відгуки":
            {
                return;
            }
        }
    }
}