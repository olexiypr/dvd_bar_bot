using DvdBarBot.States;
using Telegram.Bot.Types;

namespace DvdBarBot.Admin.AdminStates;

public abstract class AdminState
{
    public virtual async void HandleMessage(Admin admin, Update update)
    {
        ChangeState(admin, update);
    }
    public abstract void ChangeState(Admin admin, Update update);
}