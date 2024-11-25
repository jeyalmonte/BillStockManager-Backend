using SharedKernel.Interfaces;

namespace Domain.Billing.Repositories;
public interface IInvoiceRepository : IRepository<Invoice>
{
	Task<Invoice?> GetByIdWithCustomerAsync(Guid id, CancellationToken cancellationToken = default);
}
