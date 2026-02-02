using ECommerece.Domain.Entities;

namespace ECommerece.Domain.Contracts;

public interface IGenericRepository<T, TKey> where T : BaseEntity<TKey>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetAllAsync(ISpecifications<T, TKey> specifications);
    Task<T?> GetByIdAsync(TKey id);
    Task<T?> GetByIdAsync(ISpecifications<T,TKey> specifications);

    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
}
