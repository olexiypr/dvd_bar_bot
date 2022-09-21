using System.Text.RegularExpressions;
using DvdBarBot.DataBase;
using DvdBarBot.Entities;
using DvdBarBot.States;
using Telegram.Bot.Types;
using User = DvdBarBot.Entities.User;

namespace DvdBarBot.Admin.AdminStates;

public class AppProductState : AdminState
{
    private Product Product { get; set; }
    private Message FinalProductMessage { get; set; }
    private delegate Task OnCreating(Admin admin);
    public AppProductState(Admin admin)
    {
        OnCreating creating = CreatingAsync;
        creating.Invoke(admin);
    }

    private async Task CreatingAsync(Admin admin)
    {
        await AdminSender.SentSubmitToAddProducts();
    }
    public override async void ChangeState(Admin admin, Update update)
    {
        if (update.Message is { } message)
        {
            if (message.Text == "/exit")
            {
                admin.State = new NeutralState(admin);
                return;
            }
            await HandleTextMessage(message);
        }
        else if (update.CallbackQuery is { } callbackQuery)
        {
            await HandleCallbackQuery(callbackQuery);
        }
    }

    private async Task HandleTextMessage(Message message)
    {
        var messageText = message.Text;
        var walidThreeLines = new Regex(@"\n\w+");
        if (walidThreeLines.Matches(messageText).Count != 2)
        {
            await AdminSender.SendInvalidDataWarning();
            return;
        }
        var productName = messageText.Split("\n")[0];
        var productPrice = messageText.Split("\n")[1];
        var productDescription = messageText.Split("\n")[2];
        Product = new Product(productName, decimal.Parse(productPrice), productDescription);
        FinalProductMessage = await AdminSender.SendAddedProductInfoAsync(Product);
    }
    private async Task HandleCallbackQuery(CallbackQuery callbackQuery)
    {
        await using var dbContext = new ApplicationDbContext();
        var categoryId = int.Parse(callbackQuery.Data.Split("_")[1]);
        var categories = dbContext.ProductCategories.ToList();
        Product.Category = categories.First(category => category.Id == categoryId);
        await dbContext.AddAsync(Product);
        await dbContext.SaveChangesAsync();
        await AdminSender.ChangeMessageForSubmitAddProduct(FinalProductMessage);
    }
}