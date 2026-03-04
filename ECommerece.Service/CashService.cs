using ECommerece.Domain.Contracts;
using ECommerece.ServiceAbstraction;
using System.Text.Json;

namespace ECommerece.Service;

public class CashService : ICashService
{
    private readonly ICashRepository _cashRepository;

    public CashService(ICashRepository cashRepository)
    {
        _cashRepository = cashRepository;
    }
    public async Task<string?> GetCashAsync(string cashKey)
    {
        return await _cashRepository.GetCashAsync(cashKey);
    }

    public async Task SetCashAsync(string cashKey, object cashValue, TimeSpan timeToLive)
    {
        var value = JsonSerializer.Serialize(cashValue);
        await _cashRepository.SetCashAsync(cashKey, value, timeToLive);
    }
}
