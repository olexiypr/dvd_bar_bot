namespace DvdBarBot.Entities;

public class Product
{
    private static int _counter = 1;
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public int CountInStock { get; set; }
    public ProductCategory Category { get; set; }

    public Product()
    {
        
    }
    public Product(string name, decimal price, string description, int countInStock, ProductCategory category)
    {
        Id = _counter;
        _counter++;
        Name = name;
        Price = price;
        Description = description;
        Category = category;
        Category.AddProduct(this);
        CountInStock = countInStock;
    }
}