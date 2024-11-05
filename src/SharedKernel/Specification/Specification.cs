using SharedKernel.Enums;
using System.Linq.Expressions;

namespace SharedKernel.Specification;
public abstract class Specification<TEntity>
{
    protected Specification() { }

    public Expression<Func<TEntity, bool>>? Criteria { get; set; }
    public Expression<Func<TEntity, object>>? SortBy { get; private set; }
    public OrderType OrderBy { get; private set; } = OrderType.Ascending;

    protected void AddCriteria(Expression<Func<TEntity, bool>> predict)
    {
        if (Criteria is null)
        {
            Criteria = predict;
        }
        else
        {
            var left = Criteria.Parameters[0];
            var visitor = new ReplaceParameterVisitor(predict.Parameters[0], left);
            var right = visitor.Visit(predict.Body);
            Criteria = Expression.Lambda<Func<TEntity, bool>>(Expression.AndAlso(Criteria.Body, right), left);
        }
    }

    protected void SetOrder(Expression<Func<TEntity, object>> sortBy, OrderType orderBy)
    {
        SortBy = sortBy;
        OrderBy = orderBy;
    }

    private class ReplaceParameterVisitor(ParameterExpression oldParameter, ParameterExpression newParameter) : ExpressionVisitor
    {
        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (ReferenceEquals(node, oldParameter))
            {
                return newParameter;
            }

            return base.VisitParameter(node);
        }
    }
}

