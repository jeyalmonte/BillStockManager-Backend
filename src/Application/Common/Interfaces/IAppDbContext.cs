using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace Application.Common.Interfaces;

public interface IAppDbContext
{
	DbSet<TEntity> Set<TEntity>() where TEntity : class, IEntity;
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}


