using DvdBarBot.Entities;
using DvdBarBot.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace DvdBarBot.DataBase;

public class DataBase : IDbContext
{
    public DataBase()
    {
        dbContext = new ApplicationDbContext();
    }
    public ApplicationDbContext dbContext { get; set; }

    public void FillDb()
    {
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
        var elf1500 = new ProductCategory("Ельф бар 1500");
        var elf2000 = new ProductCategory("Ельф бар 2000");
        var elf5000 = new ProductCategory("Ельф бар 5000");

        var efl1500PinkLemonade = new Product("Ельф бар 1500", 280, "Pink lemonade", elf1500);
        var efl2000PinkLemonade = new Product("Ельф бар 2000", 340, "Pink lemonade",elf2000);
        var efl5000PinkLemonade = new Product("Ельф бар 5000", 500, "Pink lemonade", elf5000);

        
        elf1500.AddProduct(efl1500PinkLemonade);
        elf2000.AddProduct(efl2000PinkLemonade);
        elf2000.AddProduct(efl5000PinkLemonade);

        dbContext.Add(elf1500);
        dbContext.Add(elf2000);
        dbContext.Add(elf5000);

        dbContext.Add(efl1500PinkLemonade);
        dbContext.Add(efl2000PinkLemonade);
        dbContext.Add(efl5000PinkLemonade);
        
        dbContext.SaveChanges();
    }
}