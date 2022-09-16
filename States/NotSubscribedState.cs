using System.Diagnostics;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static DvdBarBot.Sender.Sender;
using User = DvdBarBot.Entities.User;
namespace DvdBarBot.States;
/// <summary>
/// The state that is assigned to the user if he is not subscribed to the channel
/// </summary>
public class NotSubscribedState : State
{
    private delegate Task OnCreating(User user);
    public NotSubscribedState(User user)
    {
        OnCreating proposition = PropositionSubscribe;
        proposition.Invoke(user);
    }
    /// <summary>
    /// Method that sends an offer to the user to subscribe to the channel
    /// </summary>
    /// <param name="user"></param>
    private async Task PropositionSubscribe(User user)
    {
        await SendPropositionSubscribeChannelAsync(user);
    }
    /// <summary>
    /// Method that sends an offer to the user to subscribe to the channel (If yes, change its state to States.SubscribedState)
    /// </summary>
    /// <param name="user"></param>
    /// <param name="update"></param>
    public override void ChangeState(User user, Update update)
    {
        if (update.Message is not {Text: { } messageText})
            return;
        var message = update.Message;
        if (messageText == "/start")
        {
            if (Handlers.IsSubscriberAsync(message).Result)
            {
                user.State = new SubscribedState(user);
            }
            else
            {
                user.State = new NotSubscribedState(user);
            }
        }
    }
}