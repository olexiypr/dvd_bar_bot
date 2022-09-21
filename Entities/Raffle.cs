using System.ComponentModel.DataAnnotations.Schema;
using System.Timers;
using DvdBarBot.DataBase;
using Telegram.Bot;
using Timer = System.Timers.Timer;

namespace DvdBarBot.Entities;

public class Raffle //додати функціонал для повторного розіграшу
{
    public static Raffle Instance { get; set; }
    private static object syncRoot = new Object();
    public int Id { get; set; }
    private static int _counter = 1;
    public List<User> Users { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public int MaxUsersCount { get; set; }
    public bool IsStarted { get; set; }
    public int CurrentUserCount { get; set; }
    public Promocode Promocode { get; set; }
    
    public static Timer Timer;
    public User Winner { get; set; }

    public Raffle()
    {
        
    }
    public static Raffle CreateNewInstance(Promocode promocode, int maxUserCount)
    {
        return Instance = new Raffle(promocode, maxUserCount);
    }

    public static Raffle GetInstance(Promocode promocode, int maxUserCount)
    {
        if (Instance == null)
        {
            lock (syncRoot)
            {
                Instance ??= new Raffle(promocode, maxUserCount);
            }
        }
        return Instance;
    }
    private Raffle(Promocode promocode, int maxUserCount)
    {
        IsStarted = false;
        Id = _counter;
        _counter++;
        Users = new List<User>();
        CurrentUserCount = 0;
        MaxUsersCount = maxUserCount;
        Promocode = promocode;
    }

    public void StartRaffle()
    {
        IsStarted = true;
        Start = DateTime.Now;
        double countMiliCesInDay = 1000 * 300;
        Timer = new Timer(countMiliCesInDay);
        Timer.Elapsed += GetWinner;
        Timer.Enabled = true;
        End = DateTime.Now.AddDays(1);
    }

    public async void GetWinner(Object timer, ElapsedEventArgs  e)
    {
        Timer.Dispose();
        Timer.Stop();
        /*Winner = Users[new Random().Next(MaxUsersCount)];*/
        Winner = Users[0];
        try
        {
            await using var dContext = new ApplicationDbContext();
            var promo = dContext.promocodes.First();
            await Sender.Sender.botClient.SendTextMessageAsync(chatId: Winner.ChatId,
                text: $"U are winner! Promo: {promo.Code}",
                cancellationToken: Sender.Sender.cancellationToken);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }
        finally
        {
            await Sender.Sender.botClient.SendTextMessageAsync(chatId: Winner.ChatId,
                text: $"U are winner!",
                cancellationToken: Sender.Sender.cancellationToken);
        }
    }
    public void AddUser(User user)
    {
        Users.Add(user);
        user.Raffle = this;
        CurrentUserCount++;
    }
}