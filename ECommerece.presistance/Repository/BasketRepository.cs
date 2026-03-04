using ECommerece.Domain.Contracts;
using ECommerece.Domain.Entities.BasketModule;
using StackExchange.Redis;
using System.Text.Json;

namespace ECommerece.presistance.Repository;

public class BasketRepository : IBasketRepository
{
    private readonly IDatabase _database;

    public BasketRepository(IConnectionMultiplexer connection)
    {
        _database = connection.GetDatabase();
    }
    public async Task<CustomerBasket> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan timeToLive = default)
    {
        var jsonBasket = JsonSerializer.Serialize(basket);
        var isCreatedOrUpdated = await _database.StringSetAsync(basket.Id, jsonBasket, 
            (timeToLive == default)?TimeSpan.FromDays(7):timeToLive);
        if (isCreatedOrUpdated)
        {
            return await GetBasketAsync(basket.Id);
        }
        else 
        { 
            return null;
        }
    }


    public async Task<bool> DeleteBasketAsync(string basketId) =>
        await _database.KeyDeleteAsync(basketId);

    public async Task<CustomerBasket?> GetBasketAsync(string basketId)
    {
        var basketFromDb = await _database.StringGetAsync(basketId);
        if (basketFromDb.IsNullOrEmpty)
        {
            return null;
        };
        return JsonSerializer.Deserialize<CustomerBasket>(basketFromDb!);
    }
}
