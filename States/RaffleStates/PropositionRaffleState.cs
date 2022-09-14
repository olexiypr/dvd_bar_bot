using Telegram.Bot.Types;
using User = DvdBarBot.Entities.User;

namespace DvdBarBot.States.RaffleStates;

public class PropositionRaffleState : State
{
    private delegate Task OnCreating(User user);

    private Message sentMessage;
    public PropositionRaffleState(User user)
    {
        OnCreating creating = Creating;
        creating.Invoke(user);
    }
    private async Task Creating(User user)
    {
       sentMessage = await Sender.Sender.SendPropositionToRaffleAsync(user);
    }
    public override async void ChangeState(User user, Update update)
    {
        if (update.CallbackQuery is not { } callbackQuery)
        {
            user.State = new SubscribedState(user);
            user.ProcessUpdate(update);
            return;
        }

        if (callbackQuery.Data.StartsWith("take_part_raffle"))
        {
            var raffle = await Sender.Sender.GetRaffle.GetRaffleAsync();
            try
            {
                raffle.AddUser(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            user.State = new RaffleState(user);
        }
    }
}