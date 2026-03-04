using ECommerece.Shared;
using ECommerece.Shared.CommonResult;
using ECommerece.Shared.ProductDTOS;

namespace ECommerece.ServiceAbstraction;

public interface IProductService
{
    Task<IEnumerable<ProductDTO>> GetAllProductsAsync(ProductQueryParams queryParams);
    Task<Result<ProductDTO>> GetProductByIdAsync(int id);
    Task<IEnumerable<BrandDTO>> GetAllBrandAsync();
    Task<IEnumerable<TypeDTO>> GetAllTypeAsync();
}
