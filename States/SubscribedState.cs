using DvdBarBot.States.CatalogStates;
using Telegram.Bot.Types;
using static DvdBarBot.Sender.Sender;
using User = DvdBarBot.Entities.User;

namespace DvdBarBot.States;

public class SubscribedState : State
{
    public delegate Task OnCreating (User user);
    

    public SubscribedState(User user)
    {
        OnCreating onCreating = Creating;
        onCreating.Invoke(user);
    }
    
    public SubscribedState(User user, bool isNeedInvokeCreating)
    {
        if (isNeedInvokeCreating)
        {
            OnCreating onCreating = Creating;
            onCreating.Invoke(user);
        }
    }
    public async Task Creating(User user)
    {
        await SayHalloAsync(user);
        await SendMenuAsync(user);
    }
    
    public override async void ChangeState(User user, Update update)
    {
        if (update.CallbackQuery is { } callbackQuery)
        {
            int count = 0;
            if (callbackQuery.Data.StartsWith("take_part_raffle"))
            {
                count++;
                if (count == 5)
                {
                    count = 0;
                    await Sender.Sender.SendInfoForAlreadyParticipatingRaffleAsync(user);
                }
            }
            return;
        }
        if (update.Message is not {Text: { } messageText})
            return;
        switch (messageText)
        {
            case "Каталог":
            {
                user.State = new CatalogState(user);
                return;
            }
            case "Пропозиції та акції":
            {
                return;
            }
            case "Відгуки":
            {
                return;
            }
        }
    }
}