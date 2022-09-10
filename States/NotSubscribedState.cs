using System.Diagnostics;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static DvdBarBot.Sender.Sender;
using User = DvdBarBot.Entities.User;
namespace DvdBarBot.States;

public class NotSubscribedState : State
{
    private delegate Task OnCreating(User user);
    public NotSubscribedState(User user)
    {
        OnCreating proposition = PropositionSubscribe;
        proposition.Invoke(user);
    }

    private async Task PropositionSubscribe(User user)
    {
        await PropositionSubscribeChannelAsync(user);
    }
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