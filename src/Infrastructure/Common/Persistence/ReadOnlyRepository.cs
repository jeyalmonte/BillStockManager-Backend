using Domain.Common;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Specification;

namespace Infrastructure.Common.Persistence;

public class ReadOnlyRepository<TEntity>(AppDbContext dbContext)
	: RepositoryProperties<TEntity>(dbContext), IReadOnlyRepository<TEntity> where TEntity : Entity
{
	public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
		=> await SetAsNoTracking.ToListAsync(cancellationToken);

	public Task<TEntity?> GetAsync(Specification<TEntity> specification, CancellationToken cancellationToken = default)
		=> SetAsNoTracking.Specify(specification).SingleOrDefaultAsync(cancellationToken);

	public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
		=> await SetAsNoTracking.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

	public async Task<(int TotalCount, IReadOnlyList<TEntity> Data)> ListAsync(Specification<TEntity> specification, CancellationToken cancellationToken = default)
	{
		var query = SetAsNoTracking.Specify(specification);

		var totalCount = 0;

		if (specification.IsPagingEnabled)
		{
			totalCount = await query.CountAsync(cancellationToken);
			query = query.Skip(specification.Skip).Take(specification.Take);
		}

		var data = await query.ToListAsync(cancellationToken);

		return (totalCount, data);
	}
}
