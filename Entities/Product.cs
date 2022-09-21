using DvdBarBot.DataBase;
using Microsoft.EntityFrameworkCore;

namespace DvdBarBot.Entities;

public class Product
{
    public int Id { get; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public ProductCategory Category { get; set; }

    public Product()
    {
        
    }
    public Product(string name, decimal price, string description, ProductCategory category)
    {
        Id = GetIDAsync().Result;
        Name = name;
        Price = price;
        Description = description;
        Category = category;
        Category.AddProduct(this);
    }

    private async Task<int> GetIDAsync()
    {
        var id = 0;
        try
        {
            await using var dbContext = new ApplicationDbContext();
            var product = await dbContext.products.OrderBy(product1 => product1.Id).LastAsync();
            id = product.Id + 1;
        }
        catch (Exception e)
        {
            Handlers.ErrorsDbLogger.Error($"GetIDAsync\n{e.Message}");
            if (id == 0)
            {
                await using var dbContext = new ApplicationDbContext();
                id = await dbContext.products.CountAsync() + 1;
            }
            Console.WriteLine(e);
        }

        if (id == 0)
        {
            id = new Random().Next(1000, 2000);
        }
        return id;
    }
    public Product(string name, decimal price, string description)
    {
        Id = GetIDAsync().Result;
        Name = name;
        Description = description;
        Price = price;
    }
    public override string ToString()
    {
        return $"{Name}\n" +
               $"{Description}\n" +
               $"Ціна: {Price}";
    }
}