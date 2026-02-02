using ECommerece.ServiceAbstraction;
using ECommerece.Shared;
using ECommerece.Shared.ProductDTOS;
using Microsoft.AspNetCore.Mvc;

namespace ECommerece.presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
    // GET: baseurl/api/product
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts([FromQuery]ProductQueryParams queryParams )
    {
        var products = await _productService.GetAllProductsAsync(queryParams);
        return Ok(products);
    }
    // GET: baseurl/api/product/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDTO>> GetProduct(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        return Ok(product);
    }

    // GET: baseurl/api/product/brands
    [HttpGet("brands")]
    public async Task<ActionResult<IEnumerable<BrandDTO>>> GetAllBrands()
    {
        var brands = await _productService.GetAllBrandAsync();
        return Ok(brands);
    }

    // GET: baseurl/api/product/types
    [HttpGet("types")]
    public async Task<ActionResult<IEnumerable<TypeDTO>>> GetAllTypes()
    {
        var types = await _productService.GetAllTypeAsync();
        return Ok(types);
    }
}
