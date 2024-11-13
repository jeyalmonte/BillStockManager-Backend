using Domain.Inventory;
using Domain.Inventory.Repositories;
using Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Inventory.Persistence.Repositories;
internal class ProductRepository(AppDbContext dbContext)
    : AppDbContextAccess<Product>(dbContext), IProductRepository
{
    public void Add(Product entity) => Entities.Add(entity);

    public Task<Product?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
        => asNoTracking
        ? EntitiesAsNoTracking.SingleOrDefaultAsync(x => x.Id == id, cancellationToken)
        : Entities.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => EntitiesAsNoTracking.SingleOrDefaultAsync(x => x.Name == name, cancellationToken);
}
