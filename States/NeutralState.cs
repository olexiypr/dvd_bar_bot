using Telegram.Bot.Types;
using User = DvdBarBot.Entities.User;
using static DvdBarBot.Sender.Sender;

namespace DvdBarBot.States;

public class NeutralState : State
{
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