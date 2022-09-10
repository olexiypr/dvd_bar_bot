using DvdBarBot.Entities;
using DvdBarBot.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DvdBarBot.DataBase;

public class ApplicationDbContext : DbContext, IGetAllProductsInCategory, IGetAllCategories, IAddUser
{
    public DbSet<ProductCategory> productCategories { get; set; }
    public DbSet<Product> products { get; set; }
    
    public DbSet<User> users { get; set; }
    
    public IEnumerable<ProductCategory> ProductCategories => productCategories;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder()
            .AddUserSecrets<ApplicationDbContext>()
            .Build();
        var connectionStrings = config.GetConnectionString("DvdBarDb");
        optionsBuilder.UseNpgsql(connectionStrings)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging()
            .LogTo(
                Console.WriteLine,
                new[] {DbLoggerCategory.Database.Command.Name},
                LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductCategory>()
            .ToTable("product_category")
            .HasKey(category => category.Id);

        modelBuilder.Entity<ProductCategory>()
            .HasMany(category => category.Products)
            .WithOne(product => product.Category);

        modelBuilder.Entity<Product>()
            .ToTable("products")
            .HasKey(product => product.Id);

        modelBuilder.Entity<Product>()
            .HasOne(product => product.Category)
            .WithMany(category => category.Products);

        modelBuilder.Entity<User>()
            .ToTable("users")
            .HasKey(user => user.Id);
    }

    public IEnumerable<Product> GetProductsInCategory(ProductCategory category)
    {
        return products.Where(product => product.Category == category);
    }

    public IEnumerable<Product> GetProductsInCategory(int categoryId)
    {
        return products.Where(product => product.Category.Id == categoryId);
    }

    public async Task Add(User user)
    {
        if (users.Count(use => use.ChatId == user.ChatId)==0)
        {
            users.Add(user);
        }

        await SaveChangesAsync();
    }
}