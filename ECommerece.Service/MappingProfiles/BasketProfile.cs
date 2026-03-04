using AutoMapper;
using ECommerece.Domain.Entities.BasketModule;
using ECommerece.Shared.BasketDTOS;

namespace ECommerece.Service.MappingProfiles;

public class BasketProfile : Profile
{
    public BasketProfile()
    {
        CreateMap<CustomerBasket, BasketDto>()
            .ReverseMap();
            

        CreateMap<BasketItem,BasketItemDto>()
            .ReverseMap();
    }
}
