namespace ECommerece.ServiceAbstraction;

public interface ICashService
{
    Task<string?> GetCashAsync(string cashKey);
    Task SetCashAsync(string cashKey, object cashValue, TimeSpan timeToLive);
}
