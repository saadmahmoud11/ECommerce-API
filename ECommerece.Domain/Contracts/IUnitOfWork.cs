using ECommerece.Domain.Entities;

namespace ECommerece.Domain.Contracts;

public interface IUnitOfWork
{
    Task<int> SaveChangeAsync();
    IGenericRepository<T, TKey> GetRepository<T, TKey>() where T : BaseEntity<TKey>;
}
