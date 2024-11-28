using SharedKernel.Interfaces;
using SharedKernel.Specification;

namespace Domain.Billing.Repositories;
public interface IInvoiceRepository : IRepository<Invoice>
{
	Task<Invoice?> GetByIdWithCustomerAsync(Guid id, CancellationToken cancellationToken = default);

	Task<ICollection<Invoice>> GetAllBySpecAsync(Specification<Invoice> specification,
		 int pageNumber = 0,
		int pageSize = int.MaxValue,
		CancellationToken cancellationToken = default);

	Task<int> GetTotalAsync(Specification<Invoice> specification, CancellationToken cancellationToken = default);
}
