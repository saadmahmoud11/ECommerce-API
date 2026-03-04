using ECommerece.Domain.Contracts;
using Microsoft.AspNetCore.Connections;
using StackExchange.Redis;

namespace ECommerece.presistance.Repository;

public class CashRepository : ICashRepository
{
    private readonly IDatabase database;

    public CashRepository(IConnectionMultiplexer connection)
    {
        database = connection.GetDatabase();
    }
    public async Task<string?> GetCashAsync(string cashKey)
    {
        var cashValue = await database.StringGetAsync(cashKey);
        if (cashValue.IsNullOrEmpty)
            return null;
        return cashValue.ToString();
    }

    public async Task SetCashAsync(string cashKey, string cashValue, TimeSpan timeToLive)
    {
        await database.StringSetAsync(cashKey, cashValue, timeToLive);
    }
}
