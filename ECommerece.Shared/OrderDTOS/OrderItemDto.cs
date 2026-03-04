namespace ECommerece.Shared.OrderDTOS;

public class OrderItemDto
{
    public string ProductName {  get; init; } = string.Empty;
    public string PictureUrl {  get; init; } = string.Empty;
    public decimal Price {  get; init; }
    public int Quantity { get; init; }
}
