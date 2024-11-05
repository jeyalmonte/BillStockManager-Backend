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
	public async Task<Customer?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
	   => asNoTracking
		? await EntitiesAsNoTracking.SingleOrDefaultAsync(x => x.Id == id, cancellationToken)
		: await Entities.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

	public Task<Customer?> GetByDocumentAsync(string document,
		CancellationToken cancellationToken = default)
		=> EntitiesAsNoTracking.SingleOrDefaultAsync(x => x.Document == document, cancellationToken);

	public async Task<ICollection<Customer>> GetAllBySpecAsync(Specification<Customer> specification,
		int pageNumber = 0,
		int pageSize = int.MaxValue,
		CancellationToken cancellationToken = default)
		=> await EntitiesAsNoTracking
			.Specify(specification)
			.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);

	public async Task<int> GetTotalAsync(Specification<Customer> specification, CancellationToken cancellationToken = default)
		=> await EntitiesAsNoTracking
			.Specify(specification)
			.CountAsync(cancellationToken);
}
