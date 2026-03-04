using ECommerece.Shared.BasketDTOS;
using ECommerece.Shared.CommonResult;

namespace ECommerece.ServiceAbstraction;

public interface IPaymentService
{
    Task<Result<BasketDto>> CreatePaymentIntentAsync(string basketId);
}
