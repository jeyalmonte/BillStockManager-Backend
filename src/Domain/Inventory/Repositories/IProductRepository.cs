using SharedKernel.Interfaces;

namespace Domain.Inventory.Repositories;
public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}
