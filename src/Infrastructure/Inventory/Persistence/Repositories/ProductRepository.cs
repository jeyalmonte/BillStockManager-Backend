using Domain.Inventory;
using Domain.Inventory.Repositories;
using Infrastructure.Common.Extensions;
using Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Specification;

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

	public async Task<ICollection<Product>> GetAllBySpecAsync(Specification<Product> specification,
		int pageSize = 0,
		int pageNumber = 0,
		CancellationToken cancellationToken = default)
		=> await EntitiesAsNoTracking
			.Specify(specification)
			.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);

	public async Task<int> GetTotalAsync(Specification<Product> specification, CancellationToken cancellationToken = default)
		=> await EntitiesAsNoTracking
			.Specify(specification)
			.CountAsync(cancellationToken);

	public async Task<ICollection<Product>> GetByIdsAsync(List<Guid> ids,
		bool asNoTracking = true,
		CancellationToken cancellationToken = default)
		=> await (asNoTracking ? EntitiesAsNoTracking : Entities)
			.Where(x => ids.Contains(x.Id))
			.ToListAsync(cancellationToken);
}
