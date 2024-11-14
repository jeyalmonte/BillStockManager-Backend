using SharedKernel.Interfaces;

namespace Domain.Inventory.Repositories;
public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetByNameAsync(string name,
        CancellationToken cancellationToken = default);

    Task<ICollection<Category>> GetAllAsync(CancellationToken cancellationToken = default);
}
