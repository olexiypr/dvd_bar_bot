using System.ComponentModel.DataAnnotations.Schema;
using System.Timers;
using DvdBarBot.States;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Timer = System.Timers.Timer;

namespace DvdBarBot.Entities;

public class User
{
    public int Id { get; set; }
    private static int _counter = 1;
    public long UserId { get; set; }
    public long ChatId { get; set; }
    public State State { get; set; }
    public bool IsSubscriber => Handlers.IsSubscriberAsync(this).Result;
    public Telegram.Bot.Types.User? TelegramUser { get; set; }
    public string Name { get; set; }
    [NotMapped]
    public Timer Timer { get; set; }
    public Raffle? Raffle { get; set; }

    public User()
    {
        
    }
    public User(long chatId, Telegram.Bot.Types.User? user)
    {
        Id = _counter;
        _counter++;
        UserId = user.Id;
        ChatId = chatId;
        TelegramUser = user;
        Name = user.FirstName + " " + user.LastName;
        State = new NeutralState();
        const long msSecInHour = 1000 * 60 * 60;
        var random = new Random();
        /*Timer = new Timer(msSecInHour * random.Next(12,15));
        Timer.Elapsed += OnTimerEvent;
        Timer.Enabled = true;*/
    }
    public void ProcessUpdate(Update update)
    {
        State.ChangeState(this, update);
    }

    private async void OnTimerEvent(Object source, ElapsedEventArgs eventArgs)
    {
        const long msSecInHour = 1000 * 60 * 60;
        if (DateTime.Now.Hour > 23 && DateTime.Now.Hour < 10)
        {
            Timer.Stop();
            Timer.Dispose();
        }
        else
        {
            Timer = new Timer(msSecInHour * 4);
            Timer.Elapsed += OnTimerEvent;
            Timer.Enabled = true;
        }
        if (DateTime.Now.Hour < 23 && DateTime.Now.Hour > 10)
        {
            
        }
    }
}