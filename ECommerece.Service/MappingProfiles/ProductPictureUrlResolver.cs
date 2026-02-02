using AutoMapper;
using ECommerece.Domain.Entities.ProductModule;
using ECommerece.Shared.ProductDTOS;
using Microsoft.Extensions.Configuration;

namespace ECommerece.Service.MappingProfiles;

public class ProductPictureUrlResolver : IValueResolver<Product, ProductDTO, string>
{
    private readonly IConfiguration _configuration;

    public ProductPictureUrlResolver(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string Resolve(Product source, ProductDTO destination, string destMember, ResolutionContext context)
    {
        if (string.IsNullOrEmpty(source.PictureUrl))
        {
            return string.Empty;
        }
        if (source.PictureUrl.StartsWith("http"))
        {
            return source.PictureUrl;
        }
        var baseUrl = _configuration.GetSection("URLs")["baseUrl"];
        if(string.IsNullOrEmpty(baseUrl))
        {
            return string.Empty;
        }
        var picUrl = $"{baseUrl}/{source.PictureUrl}";
        return picUrl;
    }
}
