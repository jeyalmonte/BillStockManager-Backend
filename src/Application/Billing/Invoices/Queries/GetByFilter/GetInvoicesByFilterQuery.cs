using Application.Billing.Invoices.Contracts;
using Application.Common.Results;
using Domain.Billing;
using SharedKernel.Enums;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Pagination;

namespace Application.Billing.Invoices.Queries.GetByFilter;
public record GetInvoicesByFilterQuery(
	int? InvoiceNumber,
	string? CustomerName,
	InvoiceStatus? Status,
	string? SortBy,
	OrderType OrderBy = OrderType.Ascending
	) : PaginatedQuery, IQuery<PaginatedResult<InvoiceResponse>>;