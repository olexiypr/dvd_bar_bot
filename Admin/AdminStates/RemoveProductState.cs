using DvdBarBot.DataBase;
using DvdBarBot.Entities;
using DvdBarBot.States;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace DvdBarBot.Admin.AdminStates;

public class RemoveProductState : AdminState
{
    private delegate Task OnCreating(Admin admin);

    private List<Message> SentMessages;
    private Dictionary<Product, bool> Products;
    private ApplicationDbContext dbContext { get; set; }

    public RemoveProductState(Admin admin)
    {
        Products = new Dictionary<Product, bool>();
        OnCreating creating = Creating;
        creating.Invoke(admin);
    }

    private async Task Creating(Admin admin)
    {
        dbContext = new ApplicationDbContext();
        SentMessages = await AdminSender.SendAllProductsAsync();
    }
    public override async void ChangeState(Admin admin, Update update)
    {
        if (update.CallbackQuery is not { } callbackQuery)
        {
            admin.State = new NeutralState(admin);
            return;
        }

        if (callbackQuery.Data.StartsWith("save"))
        {
            await dbContext.SaveChangesAsync();
            await dbContext.DisposeAsync();
            await AdminSender.DeleteMessagesAsync(SentMessages);
            await AdminSender.SendSubmitToSaveChangesInDb();
            admin.State = new NeutralState(admin);
            return;
        }
        var productId = int.Parse(callbackQuery.Data.Split("_")[2]);
        var product = dbContext.products.SingleAsync(product => product.Id == productId).Result;
        if (callbackQuery.Data.StartsWith("del"))
        {
            dbContext.Entry(product).State = EntityState.Deleted;
        }
        if (callbackQuery.Data.StartsWith("ret"))
        {
            dbContext.Entry(product).State = EntityState.Modified;
        }
        var messageId = callbackQuery.Message.MessageId;
        var message = SentMessages.Find(message1 => message1.MessageId == messageId);
        message = await AdminSender.ChangeMessageAsync(message);
        SentMessages = SentMessages.Where(me => me.MessageId != message.MessageId).ToList();
        SentMessages.Add(message);
    }
}