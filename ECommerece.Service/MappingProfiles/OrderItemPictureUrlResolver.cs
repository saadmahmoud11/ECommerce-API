using AutoMapper;
using ECommerece.Domain.Entities.OrderModule;
using ECommerece.Shared.OrderDTOS;
using Microsoft.Extensions.Configuration;

namespace ECommerece.Service.MappingProfiles;

internal class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
{
    private readonly IConfiguration _configuration;

    public OrderItemPictureUrlResolver(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
    {
       
        if (string.IsNullOrEmpty(source.Product.PictureUrl))
        {
            return string.Empty;
        }
        if (source.Product.PictureUrl.StartsWith("http"))
        {
            return source.Product.PictureUrl;
        }
        var baseUrl = _configuration.GetSection("URLs")["baseURL"];
        if (string.IsNullOrEmpty(baseUrl))
        {
            return string.Empty;
        }
        return $"{baseUrl}{source.Product.PictureUrl}";
    }
}
