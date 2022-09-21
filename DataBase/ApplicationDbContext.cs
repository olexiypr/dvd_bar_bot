
#define DEBUG
using System.Data.Entity.Infrastructure;
using System.Linq;
using DvdBarBot.Entities;
using DvdBarBot.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DvdBarBot.DataBase;

public class ApplicationDbContext : DbContext, IGetAllProductsInCategory, IGetAllCategories, IAddUser, IGetRaffle, IGetAllProducts
{
    public DbSet<ProductCategory> productCategories { get; set; }
    public DbSet<Product> products { get; set; }
    
    public DbSet<User> users { get; set; }
    public DbSet<Raffle> raffle{ get; set; }
    public DbSet<Promocode> promocodes { get; set; }

    public IEnumerable<ProductCategory> ProductCategories => productCategories;
    public async Task<Raffle> GetRaffleAsync ()
    {
        if (raffle.CountAsync().Result != 0)
        {
            return await raffle.FirstAsync();
        }
        return null;
    }
    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await products.ToListAsync();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder()
            .AddUserSecrets<ApplicationDbContext>()
            .Build();
        var connectionStrings = config.GetConnectionString("DvdBarDb");
#if RELEASE
        optionsBuilder.UseNpgsql(ConnectionStrings.connectionStringAzure)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging()
            .LogTo(
                new StreamWriter("efCoreLogs.log", true).WriteLine,
                new[] {DbLoggerCategory.Database.Command.Name},
                LogLevel.Information);
#elif DEBUG
        optionsBuilder.UseNpgsql(ConnectionStrings.connectionStringAzure)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine,
                new[] {DbLoggerCategory.Database.Command.Name},
                LogLevel.Information);
#endif
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

        modelBuilder.Entity<User>()
            .HasOne(user => user.Raffle);
        
        modelBuilder.Entity<User>()
            .Ignore(user => user.State);
        
        modelBuilder.Entity<User>()
            .Ignore(user => user.SentMessage);
        
        modelBuilder.Entity<User>()
            .Ignore(user => user.TelegramUser);
        
        modelBuilder.Entity<Raffle>()
            .Ignore(raffle => raffle.Winner);
        
        modelBuilder.Entity<Raffle>()
            .Ignore(raffle => raffle.Start);
        
        modelBuilder.Entity<Raffle>()
            .Ignore(raffle => raffle.End);
        
        modelBuilder.Entity<Raffle>()
            .Ignore(raffle => raffle.IsStarted);
        
        modelBuilder.Entity<Raffle>()
            .Property(raffle => raffle.CurrentUserCount)
            .HasColumnName("current_user_count");
        
        modelBuilder.Entity<Promocode>()
            .HasKey(promocode => promocode.Id);
    }

    public async Task<IEnumerable<Product>> GetProductsInCategoryAsync(ProductCategory category)
    {
        Task<IEnumerable<Product>> Get(IEnumerable<Product> products)
        {
            return Task.FromResult(products.Where(product => product.Category == category));
        }
        return await Get(products);
    }

    public async Task<IEnumerable<Product>> GetProductsInCategoryAsync(int categoryId)
    {
        Task<IEnumerable<Product>> Get(IEnumerable<Product> products)
        {
            return Task.FromResult(products.Where(product => product.Category.Id == categoryId));
        }
        return await Get(products);
    }

    public async Task AddUserAsync(User user)
    {
        if (!await users.AnyAsync(use => use.ChatId == user.ChatId))
        {
            users.Add(user);
        }

        await SaveChangesAsync();
    }
}