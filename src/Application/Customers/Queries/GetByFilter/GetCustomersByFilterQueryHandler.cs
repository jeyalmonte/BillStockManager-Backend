﻿using Application.Common.Models;
using Application.Customers.Specifications;
using Domain.Customers.Repositories;
using Mapster;
using MediatR;
using SharedKernel.Contracts.Customers;

namespace Application.Customers.Queries.GetByFilter;
public class GetCustomersByFilterQueryHandler(ICustomerRepository customerRepository) : IRequestHandler<GetCustomersByFilterQuery, PaginatedResult<CustomerResponse>>
{
    public async Task<PaginatedResult<CustomerResponse>> Handle(GetCustomersByFilterQuery request, CancellationToken cancellationToken)
    {
        var specification = new GetCustomersByFilterSpecification(request);

        var customersTask = customerRepository.GetPaginatedByFilterAsync(
            specification: specification,
            pageNumber: request.PageNumber,
            pageSize: request.PageSize,
            cancellationToken: cancellationToken);

        var totalTask = customerRepository.GetTotal(
            specification: specification,
            cancellationToken: cancellationToken);

        await Task.WhenAll(customersTask, totalTask);

        var total = await totalTask;
        var customers = await customersTask;


        var customersResponse = customers.Adapt<IReadOnlyCollection<CustomerResponse>>();

        var result = PaginatedResult<CustomerResponse>.Create(
                items: customersResponse,
                total: total,
                pageNumber: request.PageNumber,
                pageSize: request.PageSize);

        return result;
    }
}
