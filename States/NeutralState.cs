using Telegram.Bot.Types;
using User = DvdBarBot.Entities.User;
using static DvdBarBot.Sender.Sender;

namespace DvdBarBot.States;
/// <summary>
/// The state that is assigned to the user who pressed /start for the first time
/// </summary>
public class NeutralState : State
{
    /// <summary>
    /// Handle pressing /start in first time
    /// </summary>
    /// <param name="user"></param>
    /// <param name="update"></param>
    public override async void ChangeState(User user, Update update)
    {
        if (update.Message is not {Text: { } messageText})
            return;
        if (messageText == "/start")
        {
            if (user.IsSubscriber)
            {
                user.State = new SubscribedState(user);
            }
            else
            {
                user.State = new NotSubscribedState(user);
            }
        }
        else
        {
            await SendStubAsync(user);
        }
    }
}