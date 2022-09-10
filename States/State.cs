using Telegram.Bot.Types;
using User = DvdBarBot.Entities.User;

namespace DvdBarBot.States;

public abstract class State
{
    public virtual async void HandleMessage(User user, Update update)
    {
        ChangeState(user, update);
    }
    public abstract void ChangeState(User user, Update update);
}