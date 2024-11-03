using SharedKernel.Interfaces;

namespace Domain.Customers.Repositories;
public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetByDocument(string document);
}
