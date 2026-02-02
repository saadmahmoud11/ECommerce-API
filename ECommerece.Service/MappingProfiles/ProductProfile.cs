using AutoMapper;
using ECommerece.Domain.Entities.ProductModule;
using ECommerece.Shared.ProductDTOS;
using System.Diagnostics;

namespace ECommerece.Service.MappingProfiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<ProductBrand, BrandDTO>().ReverseMap();
        CreateMap<ProductType, TypeDTO>().ReverseMap();
        CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.ProductBrand, opt => opt.MapFrom(src => src.ProductBrand.Name))
            .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType.Name))
            //.ForMember(dist => dist.PictureUrl,opt => opt.MapFrom(src => $"https://localhost:7216/{src.PictureUrl}"))
            .ForMember(dist => dist.PictureUrl, opt => opt.MapFrom<ProductPictureUrlResolver>())
            .ReverseMap();

    }
}
