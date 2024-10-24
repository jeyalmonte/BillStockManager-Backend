using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace Infrastructure.Common.Persistence;

public abstract class AppDbContextAccess<TEntity>(AppDbContext dbContext) where TEntity : Entity
{
	protected readonly AppDbContext _dbContext = dbContext;
	protected DbSet<TEntity> Entities => _dbContext.Set<TEntity>();
	protected IQueryable<TEntity> EntitiesAsNoTracking => Entities.AsNoTracking();
}
