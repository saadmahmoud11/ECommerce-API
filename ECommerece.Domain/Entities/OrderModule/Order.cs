namespace ECommerece.Domain.Entities.OrderModule;

public class Order : BaseEntity<Guid>
{
    public string UserEmail { get; set; } = default!;
    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
    public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
    public OrderAddress OrderAddress { get; set; } = default!;
    public int DeliveryMethodId { get; set; } // Foreign key to DeliveryMethod
    public DeliveryMethod DeliveryMethod { get; set; } = default!;
    public ICollection<OrderItem> OrderItems { get; set; } = [];
    public decimal Subtotal { get; set; }
    public decimal GetTotal() => Subtotal + DeliveryMethod.Price;
    public string? PaymentIntentId { get; set; }

}
