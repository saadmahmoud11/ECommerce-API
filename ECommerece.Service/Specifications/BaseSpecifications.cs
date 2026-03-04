using ECommerece.Domain.Contracts;
using ECommerece.Domain.Entities;
using System.Linq.Expressions;

namespace ECommerece.Service.Specifications;

public class BaseSpecifications<T, TKey> : ISpecifications<T, TKey> where T : BaseEntity<TKey>
{

    public Expression<Func<T, bool>>? Criteria { get; }
    protected BaseSpecifications(Expression<Func<T, bool>>? criteria)
    {
        Criteria = criteria;
    }

    #region include expressions
    public ICollection<Expression<Func<T, object>>> IncludeExpressions { get; } = [];

    protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        IncludeExpressions.Add(includeExpression);
    }
    #endregion

    #region order by
    public Expression<Func<T, object>>? OrderBy { get; private set; }

    public Expression<Func<T, object>>? OrderByDescending { get; private set; }

    protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }
    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
    {
        OrderByDescending = orderByDescExpression;
    }
    #endregion

    #region pagination
    public int Take { get; private set; }

    public int Skip { get; private set; }

    public bool IsPaginated { get; private set; }

    protected void ApplyPagination(int pageSize, int pageIndex)
    {
        IsPaginated = true;
        Take = pageSize;
        Skip = pageSize * (pageIndex - 1);
    }
    #endregion
}
