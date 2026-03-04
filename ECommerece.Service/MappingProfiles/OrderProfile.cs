using AutoMapper;
using ECommerece.Domain.Entities.OrderModule;
using ECommerece.Shared.OrderDTOS;

namespace ECommerece.Service.MappingProfiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<AddressDto, OrderAddress>()
            .ReverseMap();

        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
            .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemPictureUrlResolver>())
            .ForMember(d => d.Quantity, o => o.MapFrom(s => s.Quantity));

            CreateMap<Order, OrderToReturnDto>()
            .ForMember(d => d.DelivryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
            .ForMember(d => d.OrderStatus, o => o.MapFrom(s => s.OrderStatus.ToString()))
            .ForMember(d => d.Total, o => o.MapFrom(s => s.GetTotal()));
    }
}
