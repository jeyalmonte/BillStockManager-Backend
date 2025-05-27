using Application.Common.Results;
using Application.Customers.Contracts;
using Application.Customers.Specifications;
using Domain.Customers.Repositories;
using Mapster;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Customers.Queries.GetByFilter;
public class GetCustomersByFilterQueryHandler(ICustomerRepository customerRepository)
	: IQueryHandler<GetCustomersByFilterQuery, PaginatedResult<CustomerResponse>>
{
	private readonly ICustomerRepository _customerRepository = customerRepository;
	public async Task<Result<PaginatedResult<CustomerResponse>>> Handle(GetCustomersByFilterQuery request, CancellationToken cancellationToken)
	{
		var specification = new GetCustomersByFilterSpecification(request);

		var customers = await _customerRepository.GetAllBySpecAsync(
			specification: specification,
			pageNumber: request.PageNumber,
			pageSize: request.PageSize,
			cancellationToken: cancellationToken);

		var total = await _customerRepository.GetTotalAsync(
			specification: specification,
			cancellationToken: cancellationToken);

		var customersResponse = customers.Adapt<IReadOnlyCollection<CustomerResponse>>();

		var result = PaginatedResult<CustomerResponse>.Create(
			items: customersResponse,
			totalItems: total,
			pageNumber: request.PageNumber,
			pageSize: request.PageSize);

		return result;
	}
}
