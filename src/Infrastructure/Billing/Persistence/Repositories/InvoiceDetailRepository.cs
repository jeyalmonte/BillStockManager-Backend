using Domain.Billing;
using Domain.Billing.Repositories;
using Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Billing.Persistence.Repositories;
internal class InvoiceDetailRepository(AppDbContext dbContext)
	: AppDbContextAccess<InvoiceDetail>(dbContext), IInvoiceDetailRepository
{
	public async Task<ICollection<InvoiceDetail>> GetDetailsByInvoiceId(Guid invoiceId,
		CancellationToken cancellationToken = default)
		=> await EntitiesAsNoTracking
			.Where(x => x.InvoiceId == invoiceId)
			.Include(x => x.Product)
			.ToListAsync(cancellationToken);
}
