using AutoMapper;
using ECommerece.Domain.Contracts;
using ECommerece.Domain.Entities.ProductModule;
using ECommerece.Service.Exceptions;
using ECommerece.Service.Specifications;
using ECommerece.ServiceAbstraction;
using ECommerece.Shared;
using ECommerece.Shared.CommonResult;
using ECommerece.Shared.ProductDTOS;

namespace ECommerece.Service;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper ;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    public async Task<IEnumerable<BrandDTO>> GetAllBrandAsync()
    {
        var brands = await _unitOfWork.GetRepository<ProductBrand,int>().GetAllAsync();
        return _mapper.Map<IEnumerable<BrandDTO>>(brands);
    }

    public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync(ProductQueryParams queryParams)
    {
        var spec = new ProductWithBrandAndTypeSpecification(queryParams);
        var products = await  _unitOfWork.GetRepository<Product,int>().GetAllAsync(spec);
        return _mapper.Map<IEnumerable<ProductDTO>>(products);
    }

    public async Task<IEnumerable<TypeDTO>> GetAllTypeAsync()
    {
        var types = await  _unitOfWork.GetRepository<ProductType,int>().GetAllAsync();
        return _mapper.Map<IEnumerable<TypeDTO>>(types);
    }

    public async Task<Result<ProductDTO>> GetProductByIdAsync(int id)
    { var spec = new ProductWithBrandAndTypeSpecification(id);
        var product = await  _unitOfWork.GetRepository<Product,int>().GetByIdAsync(spec);
        if (product is null)
        {
            return Error.NotFound(
                code: "Product.NotFound",
                description: $"Product with id {id} not found"
                );
        }
        return _mapper.Map<ProductDTO>(product);
    }
}
