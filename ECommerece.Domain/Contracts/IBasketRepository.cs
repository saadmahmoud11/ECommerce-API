using ECommerece.Domain.Entities.BasketModule;

namespace ECommerece.Domain.Contracts;

public interface IBasketRepository
{
    Task<CustomerBasket?> GetBasketAsync(string basketId);
    Task<CustomerBasket> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan timeToLive = default);
    Task<bool> DeleteBasketAsync(string basketId);
}
