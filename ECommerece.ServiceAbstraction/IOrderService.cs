using ECommerece.Shared.CommonResult;
using ECommerece.Shared.OrderDTOS;

namespace ECommerece.ServiceAbstraction;

public interface IOrderService
{
    // create order for the user
    // orderdto , email => order to return dto
    Task<Result<OrderToReturnDto>> CreateOrderAsync(OrderDto orderDto, string email);
}
