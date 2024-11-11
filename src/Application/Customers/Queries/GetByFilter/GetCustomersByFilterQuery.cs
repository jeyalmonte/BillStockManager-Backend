using Application.Common.Models;
using Domain.Customers;
using SharedKernel.Contracts.Customers;
using SharedKernel.Enums;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Pagination;

namespace Application.Customers.Queries.GetByFilter;
public record GetCustomersByFilterQuery(
	string? FullName,
	string? Nickname,
	DocumentType? DocumentType,
	string? Document,
	GenderType? Gender,
	string? SortBy,
	OrderType OrderBy = OrderType.Ascending
	) : PaginatedQuery, IQuery<PaginatedResult<CustomerResponse>>;
