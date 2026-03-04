using ECommerece.Shared.BasketDTOS;

namespace ECommerece.ServiceAbstraction;

public interface IBasketService
{
    Task<BasketDto?> GetBasketAsync(string basketId);
    Task<BasketDto> CreateOrUpdateBasketAsync(BasketDto basket, TimeSpan timeToLive = default);
    Task<bool> DeleteBasketAsync(string basketId);
}
