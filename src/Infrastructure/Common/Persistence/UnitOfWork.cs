using Domain.Common;

namespace Infrastructure.Common.Persistence;
public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
	public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		=> await dbContext.SaveChangesAsync(cancellationToken);
}
