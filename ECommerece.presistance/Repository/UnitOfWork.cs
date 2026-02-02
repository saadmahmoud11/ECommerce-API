using ECommerece.Domain.Contracts;
using ECommerece.Domain.Entities;
using ECommerece.presistance.Data.DbContexts;

namespace ECommerece.presistance.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly Dictionary<Type,object> _repository = [];
    private readonly StoreDbContext _dbContext;

    public UnitOfWork( StoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public IGenericRepository<T, TKey> GetRepository<T, TKey>() where T : BaseEntity<TKey>
    {
        var entityType = typeof(T);
        if (_repository.TryGetValue(entityType, out object? repository))
        {
            return (IGenericRepository<T, TKey>)repository;
        }
        var newRepository = new GenericRepository<T, TKey>(_dbContext);
        _repository[entityType] = newRepository;
        return newRepository;

    }

    public async Task<int> SaveChangeAsync() =>
        await _dbContext.SaveChangesAsync();

}
