using AutoMapper;
using ECommerece.Domain.Contracts;
using ECommerece.Domain.Entities.BasketModule;
using ECommerece.Service.Exceptions;
using ECommerece.ServiceAbstraction;
using ECommerece.Shared.BasketDTOS;

namespace ECommerece.Service;

public class BasketService : IBasketService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IMapper _mapper;

    public BasketService(IBasketRepository basketRepository, IMapper mapper)
    {
        _basketRepository = basketRepository;
        _mapper = mapper;
    }
    public async Task<BasketDto> CreateOrUpdateBasketAsync(BasketDto basket, TimeSpan timeToLive = default)
    {
        var customerBasket = _mapper.Map<BasketDto,CustomerBasket>(basket);
        var createdOrUpdatedBasket = await _basketRepository.CreateOrUpdateBasketAsync(customerBasket, timeToLive);
        return _mapper.Map<CustomerBasket, BasketDto>(createdOrUpdatedBasket);
    }

    public async Task<bool> DeleteBasketAsync(string basketId) =>
                await _basketRepository.DeleteBasketAsync(basketId);

    public async Task<BasketDto?> GetBasketAsync(string basketId)
    {
        var basket = await _basketRepository.GetBasketAsync(basketId);
        if (basket is null)
        {
            throw new BasketNotFoundException(basketId);
        }
        return _mapper.Map<CustomerBasket,BasketDto>(basket!);
    }
}
