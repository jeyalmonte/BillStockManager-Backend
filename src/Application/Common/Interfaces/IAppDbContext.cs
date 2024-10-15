using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IAppDbContext
{
	DbSet<TEntity> Set<TEntity>() where TEntity : class;
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}


