using DvdBarBot;
using DvdBarBot.Admin;
using DvdBarBot.DataBase;
using DvdBarBot.Interfaces;
using DvdBarBot.Sender;
using DvdBarBot.States;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = DvdBarBot.Entities.User;

var botClient = new TelegramBotClient(Token.GetToken);
using var cts = new CancellationTokenSource();
Handlers.Users = new Dictionary<long, User>();
var dataBase = new DataBase();
IDbContext dbContext = dataBase;
dataBase.FillDb();
Sender.dbContext = dbContext.dbContext;
// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
};
botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

// Send cancellation request to stop bot
cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    Handlers.botClient = botClient;
    Handlers.cancellationToken = cancellationToken;
    Handlers.AddUser = Sender.dbContext;
    await Handlers.HandleUpdateAsync(update);
    
}

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}