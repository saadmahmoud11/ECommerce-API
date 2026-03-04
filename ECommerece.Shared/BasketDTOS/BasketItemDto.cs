namespace ECommerece.Shared.BasketDTOS;

public class BasketItemDto
{
    public int Id { get; set; }
    public string ProductName { get; set; } = default!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string PictureUrl { get; set; } = default!;
}
