using SharedKernel.Domain;

namespace Domain.Common;
public interface IRepositoryBase<TEntity> : IReadOnlyRepository<TEntity> where TEntity : Entity
{
    void Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}
