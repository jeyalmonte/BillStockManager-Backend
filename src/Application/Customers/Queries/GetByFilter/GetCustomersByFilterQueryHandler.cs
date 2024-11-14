using Application.Common.Models;
using Application.Customers.Specifications;
using Domain.Customers.Repositories;
using Mapster;
using SharedKernel.Contracts.Customers;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Customers.Queries.GetByFilter;
public class GetCustomersByFilterQueryHandler(ICustomerRepository customerRepository)
    : IQueryHandler<GetCustomersByFilterQuery, PaginatedResult<CustomerResponse>>
{
    public async Task<Result<PaginatedResult<CustomerResponse>>> Handle(GetCustomersByFilterQuery request, CancellationToken cancellationToken)
    {
        var specification = new GetCustomersByFilterSpecification(request);

        var customers = await customerRepository.GetAllBySpecAsync(
            specification: specification,
            pageNumber: request.PageNumber,
            pageSize: request.PageSize,
            cancellationToken: cancellationToken);

        var total = await customerRepository.GetTotalAsync(
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
