using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace Infrastructure.Common.Persistence;

public abstract class RepositoryProperties<TEntity>(AppDbContext dbContext) where TEntity : Entity
{
    protected readonly AppDbContext _dbContext = dbContext;
    protected DbSet<TEntity> Set => _dbContext.Set<TEntity>();
    protected IQueryable<TEntity> SetAsNoTracking
    {
        get => _dbContext.Set<TEntity>().AsNoTracking();
    }
}
