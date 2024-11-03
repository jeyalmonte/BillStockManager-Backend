using Domain.Customers.Repositories;
using Mapster;
using SharedKernel.Contracts.Customers;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Customers.Queries.GetById;
public class GetCustomerByIdQueryHandler(ICustomerRepository customerRepository)
    : IQueryHandler<GetCustomerByIdQuery, CustomerResponse>
{
    public async Task<Result<CustomerResponse>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetByIdAsync(request.Id);

        if (customer is null)
        {
            return Error.NotFound(description: "Customer was not found.");
        }

        return customer.Adapt<CustomerResponse>();
    }
}
