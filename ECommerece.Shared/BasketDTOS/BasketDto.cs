namespace ECommerece.Shared.BasketDTOS;

public class BasketDto
{
    public string Id { get; set; } = default!;
    public ICollection<BasketItemDto> Items { get; set; }
    public int? DeliveryMethodId { get; set; }
    public string? ClientSecret { get; set; }
    public string? PaymentIntentId { get; set; }
    public decimal ShippingCost { get; set; }
}
}
