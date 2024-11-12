using Domain.Inventory;
using Domain.Inventory.Repositories;
using Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Inventory.Persistence.Repositories;
internal class CategoryRepository(AppDbContext dbContext)
    : AppDbContextAccess<Category>(dbContext), ICategoryRepository
{
    public void Add(Category entity) => Entities.Add(entity);
    public async Task<ICollection<Category>> GetAllAsync(CancellationToken cancellationToken = default)
        => await EntitiesAsNoTracking.ToListAsync(cancellationToken);
    public async Task<Category?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
        => asNoTracking
        ? await EntitiesAsNoTracking.SingleOrDefaultAsync(x => x.Id == id, cancellationToken)
        : await Entities.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    public Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => EntitiesAsNoTracking.SingleOrDefaultAsync(x => x.Name == name, cancellationToken);
}
