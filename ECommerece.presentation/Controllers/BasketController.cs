using ECommerece.ServiceAbstraction;
using ECommerece.Shared.BasketDTOS;
using Microsoft.AspNetCore.Mvc;

namespace ECommerece.presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BasketController : ControllerBase
{
    private readonly IBasketService _basketService;

    public BasketController(IBasketService basketService)
    {
        _basketService = basketService;
    }

    // GET: baseurl/api/basket/{id}
    [HttpGet]
    public async Task<ActionResult<BasketDto>> GetBasket(string id)
    {
        var basket = await _basketService.GetBasketAsync(id);
        return Ok(basket);
    }

    // POST: baseurl/api/basket
    [HttpPost]
    public async Task<ActionResult<BasketDto>> CreateOrUpdateBasket(BasketDto basket)
    {
        var Createdbasket = await _basketService.CreateOrUpdateBasketAsync(basket);
        return Ok(Createdbasket);
    }
    // DELETE: baseurl/api/basket?id={id}
    [HttpDelete]
    public async Task<ActionResult<bool>> DeleteBasket(string id)
    {
        var result = await _basketService.DeleteBasketAsync(id);
        return Ok(result);
    }
}
