using Application.Billing.Invoices.Contracts;
using Domain.Billing.Repositories;
using Mapster;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Billing.Invoices.Queries.GetById;
public class GetInvoiceByIdQueryHandler(IInvoiceRepository invoiceRepository)
	: IQueryHandler<GetInvoiceByIdQuery, InvoiceResponse>
{
	private readonly IInvoiceRepository _invoiceRepository = invoiceRepository;
	public async Task<Result<InvoiceResponse>> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
	{
		var invoice = await _invoiceRepository.GetByIdWithCustomerAsync(
			id: request.Id,
			cancellationToken: cancellationToken);

		if (invoice is null)
		{
			return Error.NotFound(description: "Invoice not found.");
		}

		return invoice.Adapt<InvoiceResponse>();
	}
}
