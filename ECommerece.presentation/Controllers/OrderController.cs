using ECommerece.ServiceAbstraction;
using ECommerece.Shared.OrderDTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerece.presentation.Controllers;

public class OrderController :ApiBaseController
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        var order = await _orderService.CreateOrderAsync(orderDto, email!);
        return HandleResult(order);
    }
}
