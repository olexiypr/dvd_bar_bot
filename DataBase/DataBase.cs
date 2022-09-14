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

        var efl1500PinkLemonade = new Product("Ельф бар 1500", 280, "Pink lemonade", 5, elf1500);
        var efl2000PinkLemonade = new Product("Ельф бар 2000", 340, "Pink lemonade", 4,elf2000);
        var efl5000PinkLemonade = new Product("Ельф бар 5000", 500, "Pink lemonade", 6, elf5000);

        
        elf1500.AddProduct(efl1500PinkLemonade);
        elf2000.AddProduct(efl2000PinkLemonade);
        elf2000.AddProduct(efl5000PinkLemonade);

        dbContext.productCategories.Add(elf1500);
        dbContext.productCategories.Add(elf2000);
        dbContext.productCategories.Add(elf5000);

        dbContext.products.Add(efl1500PinkLemonade);
        dbContext.products.Add(efl2000PinkLemonade);
        dbContext.products.Add(efl5000PinkLemonade);
        
        dbContext.SaveChanges();
    }
}