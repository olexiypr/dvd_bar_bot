using System.Text;

namespace DvdBarBot.Entities;

public class Promocode
{
    public int Id { get; set; }
    private static int _counter = 1;
    public int Discount { get; set; }
    public string Code { get; set; }

    public Promocode()
    {
        
    }

    public Promocode(int discount)
    {
        Id = _counter;
        _counter++;
        Discount = discount;
        Code = GenerateCode();
    }

    private string GenerateCode()
    {
        const string allowedChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var random = new Random();
        var lenght = random.Next(7, 11);
        var builder = new StringBuilder();
        for (var i = 0; i < lenght; i++)
        {
            builder.Append(allowedChars[random.Next(allowedChars.Length-1)]);
        }

        return builder.ToString();
    }
}