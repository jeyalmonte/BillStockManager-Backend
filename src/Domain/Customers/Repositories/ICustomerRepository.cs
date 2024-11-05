using SharedKernel.Interfaces;
using SharedKernel.Specification;

namespace Domain.Customers.Repositories;
public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetByDocument(string document);
    Task<int> GetTotal(Specification<Customer> specification, CancellationToken cancellationToken = default);
    Task<ICollection<Customer>> GetPaginatedByFilterAsync(Specification<Customer> specification,
        int pageNumber = 0,
        int pageSize = int.MaxValue,
        CancellationToken cancellationToken = default);
}
