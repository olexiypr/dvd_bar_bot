using DvdBarBot.DataBase;
using DvdBarBot.Entities;
using Serilog;
using Telegram.Bot.Types;
using User = DvdBarBot.Entities.User;

namespace DvdBarBot.States.RaffleStates;
/// <summary>
/// The state that is assigned to all users when the admin started the raffle
/// </summary>
public class PropositionRaffleState : State
{
    private delegate Task OnCreating(User user);
    private Message? _sentMessage;
    public PropositionRaffleState(User user)
    {
        OnCreating creating = Creating;
        creating.Invoke(user);
    }
    private async Task Creating(User user)
    {
       _sentMessage = await Sender.Sender.SendPropositionToRaffleAsync(user);
    }
    /// <summary>
    /// Method that is called when user has received an offer to participate in the raffle
    /// </summary>
    /// <param name="user"></param>
    /// <param name="update"></param>
    public override async void ChangeState(User user, Update update)
    {
        if (update.CallbackQuery is not { } callbackQuery)
        {
            user.State = new SubscribedState(user);
            user.ProcessUpdate(update);
            return;
        }

        if (callbackQuery.Data.StartsWith("dont_take_part"))
        {
            await Sender.Sender.SendSubmitToDontTakePartInRaffle(user, _sentMessage);
            user.State = new SubscribedState(user);
            user.SentMessage = _sentMessage;
        }
        if (callbackQuery.Data.StartsWith("take_part_raffle"))
        {
            try
            {
                Handlers.InfoDbLogger.Information($"PropositionRaffleState.ChangeState\n{user.GetInfo()}");
                await using var dbContext = new ApplicationDbContext();
                var raffle = Raffle.Instance;
                raffle.AddUser(user);
                dbContext.Update(raffle);
                await dbContext.SaveChangesAsync();
                await Sender.Sender.SendSubmitToTakePartInRaffle(user, _sentMessage);
            }
            catch (Exception e)
            {
                Handlers.ErrorsDbLogger.Error($"PropositionRaffleState.ChangeState\n{user.GetInfo()}\n{e.Message}");
                Console.WriteLine(e.Message);
                throw;
            }
            user.State = new SubscribedState(user, false);
        }
    }
}