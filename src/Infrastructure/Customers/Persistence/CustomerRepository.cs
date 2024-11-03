using Domain.Customers;
using Domain.Customers.Repositories;
using Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Customers.Persistence;
public class CustomerRepository(AppDbContext dbContext) : AppDbContextAccess<Customer>(dbContext), ICustomerRepository
{
    public void Add(Customer entity) => Entities.Add(entity);
    public async Task<Customer?> GetByIdAsync(Guid id, bool asNoTracking = true)
       => asNoTracking
        ? await EntitiesAsNoTracking.SingleOrDefaultAsync(x => x.Id == id)
        : await Entities.SingleOrDefaultAsync(x => x.Id == id);
}
