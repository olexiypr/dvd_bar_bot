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
    private async Task Creating(User user)
    {
        await SayHalloAsync(user);
        await SendMenuAsync(user);
    }
    
    public override async void ChangeState(User user, Update update)
    {
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