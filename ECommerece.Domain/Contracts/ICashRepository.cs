namespace ECommerece.Domain.Contracts;

public interface ICashRepository
{
    Task<string?> GetCashAsync(string cashKey);
    Task SetCashAsync(string cashKey, string cashValue, TimeSpan timeToLive );
}
