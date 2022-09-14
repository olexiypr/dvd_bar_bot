using DvdBarBot.DataBase;
using DvdBarBot.Entities;
using DvdBarBot.States;
using DvdBarBot.States.RaffleStates;
using Telegram.Bot.Types;
using User = DvdBarBot.Entities.User;

namespace DvdBarBot.Admin.AdminStates;

public class RaffleState : AdminState
{
    private delegate Task onCreating(Admin admin);
    public RaffleState(Admin admin)
    {
        onCreating creating = onCreate;
        creating.Invoke(admin);
    }

    private async Task onCreate(Admin admin)
    {
        var promocode = new Promocode(10);
        admin.Raffle = Raffle.CreateNewInstance(promocode, 30);
        admin.Raffle.StartRaffle();
        var dbContext = Sender.Sender.dbContext;
        await dbContext.raffle.AddAsync(admin.Raffle);
        await dbContext.promocodes.AddAsync(promocode);
        await dbContext.SaveChangesAsync();
        foreach (var user in Handlers.Users.Values)
        {
            user.State = new PropositionRaffleState(user);
        }
    }
    public override void ChangeState(Admin admin, Update update)
    {
        
    }
}