using Microsoft.EntityFrameworkCore;
using SharedKernel.Enums;
using SharedKernel.Specification;

namespace Infrastructure.Common.Extensions;
public static class QueryableExtensions
{
    public static IQueryable<TEntity> Specify<TEntity>(this IQueryable<TEntity> queryable,
        Specification<TEntity> specification)
    {
        if (specification.Criteria is not null)
            queryable = queryable.Where(specification.Criteria);

        if (specification.SortBy is not null)
        {
            queryable = specification.OrderBy == OrderType.Descending
                 ? queryable.OrderByDescending(specification.SortBy)
                 : queryable.OrderBy(specification.SortBy);
        }

        return queryable;
    }

    public static async Task<List<TEntity>> ToPaginatedListAsync<TEntity>(this IQueryable<TEntity> queryable,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
        => await queryable
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

}
