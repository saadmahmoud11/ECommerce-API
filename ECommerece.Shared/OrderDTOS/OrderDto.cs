namespace ECommerece.Shared.OrderDTOS;

public record OrderDto(string BasketId,int DeliveryMethodId,AddressDto Address);

