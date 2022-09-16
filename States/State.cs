using Telegram.Bot.Types;
using User = DvdBarBot.Entities.User;

namespace DvdBarBot.States;

/// <summary>
/// With the help of this class, the state machine pattern is implemented
/// </summary>
public abstract class State
{
    /// <summary>
    /// Update user state
    /// </summary>
    /// <param name="user">User whose state should change</param>
    /// <param name="update">Update from user (text message or callback query)</param>
    public abstract void ChangeState(User user, Update update);
}