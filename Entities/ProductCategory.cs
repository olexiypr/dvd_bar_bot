using DvdBarBot.Interfaces;

namespace DvdBarBot.Entities;

public class ProductCategory
{
    public int Id { get; set; }
    private int _counter = 0;
    public string Name { get; set; }
    public List<Product> Products { get; set; }

    public ProductCategory()
    {
        
    }
    public ProductCategory(string name)
    {
        Id = _counter;
        _counter++;
        Name = name;
        Products = new List<Product>();
    }
    public void AddProduct(Product product)
    {
        Products.Add(product);
    }   
}