using Domain.Customers;
using Domain.Customers.Repositories;
using Infrastructure.Common.Extensions;
using Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Specification;

namespace Infrastructure.Customers.Persistence;
public class CustomerRepository(AppDbContext dbContext) : AppDbContextAccess<Customer>(dbContext), ICustomerRepository
{
    public void Add(Customer entity) => Entities.Add(entity);
    public async Task<Customer?> GetByIdAsync(Guid id, bool asNoTracking = true)
       => asNoTracking
        ? await EntitiesAsNoTracking.SingleOrDefaultAsync(x => x.Id == id)
        : await Entities.SingleOrDefaultAsync(x => x.Id == id);

    public Task<Customer?> GetByDocument(string document)
        => EntitiesAsNoTracking.SingleOrDefaultAsync(x => x.Document == document);

    public async Task<ICollection<Customer>> GetPaginatedByFilterAsync(Specification<Customer> specification,
        int pageNumber = 0,
        int pageSize = int.MaxValue,
        CancellationToken cancellationToken = default)
        => await EntitiesAsNoTracking
            .Specify(specification)
            .ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);

    public async Task<int> GetTotal(Specification<Customer> specification, CancellationToken cancellationToken = default)
        => await EntitiesAsNoTracking
            .Specify(specification)
            .CountAsync(cancellationToken);
}
