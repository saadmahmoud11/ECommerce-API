using ECommerece.Domain.Contracts;
using ECommerece.Domain.Entities;
using ECommerece.presistance.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Linq.Expressions;

namespace ECommerece.presistance.Repository;

public class GenericRepository<T,TKey> : IGenericRepository<T, TKey> where T : BaseEntity<TKey>
{
    private readonly StoreDbContext _dbContext;

    public GenericRepository(StoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task AddAsync(T entity) => 
        await _dbContext.Set<T>().AddAsync(entity);


    public async Task<IEnumerable<T>> GetAllAsync() =>
            await _dbContext.Set<T>().ToListAsync();

    public async Task<IEnumerable<T>> GetAllAsync(ISpecifications<T, TKey> specifications)
    {
        return await SpecificationsEvaluator.CreateQuery(_dbContext.Set<T>(),specifications).ToListAsync();
    }

    public async Task<T?> GetByIdAsync(TKey id) =>
        await _dbContext.Set<T>().FindAsync(id);

    public async Task<T?> GetByIdAsync(ISpecifications<T, TKey> specifications)
    {
        return await SpecificationsEvaluator
            .CreateQuery(_dbContext.Set<T>(), specifications)
            .FirstOrDefaultAsync();
    }

    public void Remove(T entity) =>
        _dbContext.Set<T>().Remove(entity);

    
    public void Update(T entity) =>
        _dbContext.Set<T>().Update(entity);

}
