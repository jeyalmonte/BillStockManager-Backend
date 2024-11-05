using SharedKernel.Interfaces;
using SharedKernel.Specification;

namespace Domain.Customers.Repositories;
public interface ICustomerRepository : IRepository<Customer>
{
	Task<Customer?> GetByDocumentAsync(string document, CancellationToken cancellationToken = default);
	Task<int> GetTotalAsync(Specification<Customer> specification, CancellationToken cancellationToken = default);
	Task<ICollection<Customer>> GetAllBySpecAsync(Specification<Customer> specification,
		int pageNumber = 0,
		int pageSize = int.MaxValue,
		CancellationToken cancellationToken = default);
}
