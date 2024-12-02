using Domain.Billing;
using Domain.Billing.Repositories;
using Infrastructure.Common.Extensions;
using Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Specification;

namespace Infrastructure.Billing.Persistence.Repositories;
internal class InvoiceRepository(AppDbContext dbContext) : AppDbContextAccess<Invoice>(dbContext), IInvoiceRepository
{
	public void Add(Invoice entity) => Entities.Add(entity);

	public async Task<Invoice?> GetByIdAsync(Guid id, bool asNoTracking = true,
		CancellationToken cancellationToken = default)
		=> asNoTracking
			? await EntitiesAsNoTracking.SingleOrDefaultAsync(e => e.Id == id, cancellationToken)
			: await Entities.SingleOrDefaultAsync(e => e.Id == id, cancellationToken);

	public async Task<Invoice?> GetByIdWithCustomerAsync(Guid id, CancellationToken cancellationToken = default)
		=> await EntitiesAsNoTracking
			.Include(e => e.Customer)
			.SingleOrDefaultAsync(e => e.Id == id, cancellationToken);

	public async Task<ICollection<Invoice>> GetAllBySpecAsync(Specification<Invoice> specification,
		int pageNumber = 0,
		int pageSize = int.MaxValue,
		CancellationToken cancellationToken = default)
		=> await EntitiesAsNoTracking
			.Include(x => x.Customer)
			.Specify(specification)
			.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);

	public async Task<int> GetTotalAsync(Specification<Invoice> specification, CancellationToken cancellationToken = default)
		=> await EntitiesAsNoTracking
			.Specify(specification)
			.CountAsync(cancellationToken);
}
