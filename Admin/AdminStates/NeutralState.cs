using DvdBarBot.States;
using Telegram.Bot.Types;
using User = DvdBarBot.Entities.User;

namespace DvdBarBot.Admin.AdminStates;

public class NeutralState : AdminState
{
    private delegate Task onCrating();
    public NeutralState(Admin admin)
    {
        onCrating crating = OnCrating;
        crating.Invoke();
    }

    private async Task OnCrating()
    {
        await AdminSender.SendMenuAsync();
    }
    public override void ChangeState(Admin admin, Update update)
    {
        var message = update.Message.Text;
        switch (message)
        {
            case "Додати товар":
            {
                return;
            }
            case "Додати категорію":
            {
                return;
            }
            case "Видалити товар":
            {
                return;
            }
            case "Видалити категорію":
            {
                return;
            }
            case "Почати роіграш":
            {
                admin.State = new RaffleState(admin);
                return;
            }
        }
    }
}