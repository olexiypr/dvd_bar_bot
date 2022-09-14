using Telegram.Bot.Types;
using User = DvdBarBot.Entities.User;

namespace DvdBarBot.States.RaffleStates;

public class RaffleState : State
{
    private delegate Task OnCreating(User user);
    public RaffleState(User user)
    {
        OnCreating creating = Creating;
        creating.Invoke(user);
    }
    private async Task Creating(User user)
    {
        await Sender.Sender.SendPropositionToRaffleAsync(user);
        if (!Sender.Sender.GetRaffle.GetRaffleAsync().Result.IsStarted)
        {
            
        }
    }
    public override void ChangeState(User user, Update update)
    {
        
    }
}