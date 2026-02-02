namespace ECommerece.Shared;

public class ProductQueryParams
{
    public int? BrandId { get; set; }
    public int? TypeId { get; set; }
    public string? Search { get; set; }
    public ProductSortingOptions Sort { get; set; }
    private int _pageIndex = 1;
    public int PageIndex
    {
        get { return _pageIndex; }
        set
        {
            if (value < 1)
                _pageIndex = 1;
            else
                _pageIndex = value;
        }
    }

    private const int DefaultPageSize = 5;
    private const int maxPageSize = 50;
    private int _pageSize = DefaultPageSize;
    public int PageSize
    {
        get { return _pageSize; }
        set
        {
            if (value < 1)
                _pageSize = DefaultPageSize;
            else if (value > maxPageSize)
                _pageSize = maxPageSize;
            else
                _pageSize = value;
        }
    }

}
