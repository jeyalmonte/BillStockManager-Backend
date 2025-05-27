using Application.Customers.Contracts;
using Domain.Customers.Repositories;
using Mapster;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Customers.Queries.GetById;
public class GetCustomerByIdQueryHandler(ICustomerRepository customerRepository)
	: IQueryHandler<GetCustomerByIdQuery, CustomerResponse>
{
	private readonly ICustomerRepository _customerRepository = customerRepository;

	public async Task<Result<CustomerResponse>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
	{
		var customer = await _customerRepository.GetByIdAsync(
			id: request.Id,
			cancellationToken: cancellationToken);

		if (customer is null)
		{
			return Error.NotFound(description: "Customer does not exist.");
		}

		return customer.Adapt<CustomerResponse>();
	}
}
