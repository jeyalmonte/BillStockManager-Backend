using SharedKernel.Domain;

namespace SharedKernel.Interfaces;
public interface IRepository<TEntity> where TEntity : class, IEntity
{
	void Add(TEntity entity);
	Task<TEntity?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default);
}
