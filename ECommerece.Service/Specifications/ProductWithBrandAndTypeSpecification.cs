using ECommerece.Domain.Entities.ProductModule;
using ECommerece.Shared;

namespace ECommerece.Service.Specifications;

public class ProductWithBrandAndTypeSpecification : BaseSpecifications<Product, int>
{
    // get product by id with its brand and type
    public ProductWithBrandAndTypeSpecification(int id) : base(p => p.Id == id)
    {
        AddInclude(p => p.ProductBrand);
        AddInclude(p => p.ProductType);
    }
    // get all products with their brands and types
    public ProductWithBrandAndTypeSpecification(ProductQueryParams queryParams)
        : base(p => (!queryParams.BrandId.HasValue || p.BrandId == queryParams.BrandId.Value) && (!queryParams.TypeId.HasValue || p.TypeId == queryParams.TypeId.Value) &&
        (string.IsNullOrEmpty(queryParams.Search) || p.Name.ToLower().Contains(queryParams.Search.ToLower())))
    {
        // branId is not null
        // typeId is not null
        //branId and typeId are not null

        AddInclude(p => p.ProductBrand);
        AddInclude(p => p.ProductType);

        // add sorting
        switch (queryParams.Sort)
        {
            case ProductSortingOptions.NameAsc:
                AddOrderBy(p => p.Name);
                break;
            case ProductSortingOptions.NameDesc:
                AddOrderByDescending(p => p.Name);
                break;
            case ProductSortingOptions.PriceAsc:
                AddOrderBy(p => p.Price);
                break;
            case ProductSortingOptions.PriceDesc:
                AddOrderByDescending(p => p.Price);
                break;
            default:
                AddOrderBy(p => p.Name);
                break;

        }

        // add pagination

        ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
    }
}
