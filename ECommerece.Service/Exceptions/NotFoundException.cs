namespace ECommerece.Service.Exceptions;

public abstract class NotFoundException(string message) : Exception(message)
{
}
public sealed class ProductNotFoundException(int id) : NotFoundException($"Product with id {id} not found");
public sealed class BasketNotFoundException(string id) : NotFoundException($"Basket with id {id} not found");
