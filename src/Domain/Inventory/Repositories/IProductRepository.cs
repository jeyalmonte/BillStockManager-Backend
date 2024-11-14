using SharedKernel.Interfaces;
using SharedKernel.Specification;

namespace Domain.Inventory.Repositories;
public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetByNameAsync(string name,
        CancellationToken cancellationToken = default);

    Task<ICollection<Product>> GetAllBySpecAsync(Specification<Product> specification,
        int pageSize = 0,
        int pageNumber = 0,
        CancellationToken cancellationToken = default);

    Task<int> GetTotalAsync(Specification<Product> specification,
        CancellationToken cancellationToken = default);
}
