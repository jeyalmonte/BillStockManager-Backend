using Application.Billing.Invoices.Specifications;
using Application.Common.Results;
using Domain.Billing.Repositories;
using Mapster;
using SharedKernel.Contracts.Invoices;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Billing.Invoices.Queries.GetByFilter;
public class GetInvoicesByFilterQueryHandler(IInvoiceRepository invoiceRepository)
	: IQueryHandler<GetInvoicesByFilterQuery, PaginatedResult<InvoiceResponse>>
{
	public async Task<Result<PaginatedResult<InvoiceResponse>>> Handle(GetInvoicesByFilterQuery request, CancellationToken cancellationToken)
	{
		var specification = new GetInvoicesByFilterSpecification(request);

		var total = await invoiceRepository.GetTotalAsync(
			specification: specification,
			cancellationToken: cancellationToken);

		var invoices = await invoiceRepository.GetAllBySpecAsync(
			specification: specification,
			pageNumber: request.PageNumber,
			pageSize: request.PageSize,
			cancellationToken: cancellationToken);

		var invoicesResponse = invoices.Adapt<IReadOnlyCollection<InvoiceResponse>>();

		return PaginatedResult<InvoiceResponse>.Create(
			items: invoicesResponse,
			totalItems: total,
			pageNumber: request.PageNumber,
			pageSize: request.PageSize);
	}
}
