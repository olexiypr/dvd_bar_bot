namespace DvdBarBot.Entities;

public class Raffle
{
    public List<User> Users { get; set; }
    public DateTime start { get; set; }
    public DateTime end { get; set; }
    public int MaxUsersCount { get; set; }

    public Raffle()
    {
        Users = new List<User>();
        start = DateTime.Now;
        end
    }
}