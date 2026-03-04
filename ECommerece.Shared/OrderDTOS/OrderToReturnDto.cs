namespace ECommerece.Shared.OrderDTOS;

public class OrderToReturnDto
{
    public Guid Id { get; set; }
    public string UserEmail { get; init; } = string.Empty;
    public ICollection<OrderItemDto> Items { get; init; } = [];
    public AddressDto OrderAddress { get; set; } = null!;
    public string DelivryMethod { get; init; } = string.Empty;
    public string OrderStatus {  get; init; } = string.Empty;
    public DateTimeOffset OrderDate { get; init; }
    public decimal SubTotal { get; init; }
    public decimal Total { get; init; }
}
