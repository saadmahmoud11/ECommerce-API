using ECommerece.Domain.Contracts;
using ECommerece.Domain.Entities;
using ECommerece.Domain.Entities.OrderModule;
using ECommerece.Domain.Entities.ProductModule;
using ECommerece.presistance.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ECommerece.presistance.DataSeed;

public class DataInitializer : IDataInitializer
{
    private readonly StoreDbContext _dbContext;

    public DataInitializer(StoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task InitializeAsync()
    {
        try
        {
            var hasProducts = await _dbContext.Products.AnyAsync();
            var hasbrands = await _dbContext.ProductBrands.AnyAsync();
            var hastypes = await _dbContext.ProductTypes.AnyAsync();
            var hasDeliveryMethods = await _dbContext.Set<DeliveryMethod>().AnyAsync();
            if (hasProducts && hasbrands && hastypes && hasDeliveryMethods) return;
            if (!hasbrands)
            {
                await SeedDataFromJson<ProductBrand, int>("brands.json", _dbContext.ProductBrands);
            }
            if (!hastypes)
            {
                await SeedDataFromJson<ProductType, int>("types.json", _dbContext.ProductTypes);
                await _dbContext.SaveChangesAsync();
            }
            if (!hasProducts)
            {
                await SeedDataFromJson<Product, int>("products.json", _dbContext.Products);
                await _dbContext.SaveChangesAsync();
            }
            if (!hasDeliveryMethods)
            {
                await SeedDataFromJson<DeliveryMethod, int>("delivery.json", _dbContext.Set<DeliveryMethod>());
                await _dbContext.SaveChangesAsync();
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during data seeding {ex}");
        }
    }

    private async Task SeedDataFromJson<T, TKey>(string fileName, DbSet<T> dbSet) where T : BaseEntity<TKey>
    { //C:\Users\EL-Manfy\source\repos\ECommerce\ECommerece.presistance\Data\JSONFiles\
        var filePath = @"..\ECommerece.presistance\Data\JSONFiles\" + fileName;
        if (!File.Exists(filePath)) throw new FileNotFoundException($"The file {filePath} was not found.");
        try
        {
            using var dataSream = File.OpenRead(filePath);
            var data = await JsonSerializer.DeserializeAsync<List<T>>(dataSream, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
            if (data is not null)
            {
                await dbSet.AddRangeAsync(data);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while reading json file {ex}");
        }
    }
}
