namespace ECommerece.Domain.Entities.BasketModule;

public class CustomerBasket
{
    public string Id { get; set; } = default!;
    public ICollection<BasketItem> Items { get; set; } = [];
    public int? DeliveryMethodId { get; set; }
    public string? ClientSecret { get; set; }
    public string? PaymentIntentId { get; set; }
    public decimal ShippingCost { get; set; }
}
