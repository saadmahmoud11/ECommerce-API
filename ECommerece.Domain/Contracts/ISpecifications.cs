using ECommerece.Domain.Entities;
using System.Linq.Expressions;

namespace ECommerece.Domain.Contracts;

public interface ISpecifications<T, TKey> where T : BaseEntity<TKey>
{
    public ICollection<Expression<Func<T, object>>> IncludeExpressions { get; }
    public Expression<Func<T, bool>>? Criteria { get; }
    public Expression<Func<T, object>> OrderBy { get; }
    public Expression<Func<T, object>> OrderByDescending { get; }
    public int Take { get; }
    public int Skip { get; }
    public bool IsPaginated{ get; }
}
