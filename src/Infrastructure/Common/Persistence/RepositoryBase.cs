using Domain.Common;
using SharedKernel.Domain;

namespace Infrastructure.Common.Persistence;

public class RepositoryBase<TEntity>(AppDbContext dbContext)
    : ReadOnlyRepository<TEntity>(dbContext), IRepositoryBase<TEntity> where TEntity : Entity
{
    public void Add(TEntity entity) => Set.Add(entity);
    public void AddRange(IEnumerable<TEntity> entities) => Set.AddRange(entities);
    public void Delete(TEntity entity) => Set.Remove(entity);
    public void Update(TEntity entity) => Set.Update(entity);
}
