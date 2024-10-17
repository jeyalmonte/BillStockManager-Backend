using SharedKernel.Interfaces;

namespace Infrastructure.Common.Persistence;
public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
	public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
		=> await dbContext.SaveChangesAsync(cancellationToken);
}
