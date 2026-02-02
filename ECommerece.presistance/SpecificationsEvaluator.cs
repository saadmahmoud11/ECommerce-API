using ECommerece.Domain.Contracts;
using ECommerece.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerece.presistance;

public static class SpecificationsEvaluator
{
    public static IQueryable<T> CreateQuery<T,TKey>(IQueryable<T> entryPoint ,
        ISpecifications<T, TKey> specifications) where T : BaseEntity<TKey>
    {
        var query = entryPoint; // _dbContext.Set<T>().AsQueryable();
        if(specifications is not null)
        {
            if (specifications.Criteria is not null)
            {
                query = query.Where(specifications.Criteria);
            }
            if (specifications.IncludeExpressions is not null && specifications.IncludeExpressions.Any())
            {
                //foreach (var include in specifications.IncludeExpressions)
                //{
                //    query = query.Where(include);
                //}
                query = specifications.IncludeExpressions
                    .Aggregate(query, (currentQuery, includeExp) => currentQuery.Include(includeExp));
            }
            if (specifications.OrderBy is not null)
            {
                query = query.OrderBy(specifications.OrderBy);
            }
            if (specifications.OrderByDescending is not null)
            {
                query = query.OrderByDescending(specifications.OrderByDescending);
            }
            if (specifications.IsPaginated)
            {
                query = query.Skip(specifications.Skip).Take(specifications.Take);
            }
        }

        return query;
    }
}
