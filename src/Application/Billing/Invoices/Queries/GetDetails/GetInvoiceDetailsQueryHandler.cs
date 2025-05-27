using Application.Billing.Invoices.Contracts;
using Domain.Billing.Repositories;
using Mapster;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Billing.Invoices.Queries.GetDetails;
public class GetInvoiceDetailsQueryHandler(
	IInvoiceRepository invoiceRepository,
	IInvoiceDetailRepository invoiceDetailRepository)
	: IQueryHandler<GetInvoiceDetailsQuery, ICollection<InvoiceDetailResponse>>
{
	private readonly IInvoiceRepository _invoiceRepository = invoiceRepository;
	private readonly IInvoiceDetailRepository _invoiceDetailRepository = invoiceDetailRepository;
	public async Task<Result<ICollection<InvoiceDetailResponse>>> Handle(GetInvoiceDetailsQuery request, CancellationToken cancellationToken)
	{
		var invoice = await _invoiceRepository.GetByIdAsync(
			id: request.Id,
			cancellationToken: cancellationToken);

		if (invoice is null)
		{
			return Error.NotFound(description: "Invoice not found.");
		}

		var details = await _invoiceDetailRepository.GetDetailsWithProductByInvoiceIdAsync(
			invoiceId: request.Id,
			cancellationToken: cancellationToken);

		return details.Adapt<List<InvoiceDetailResponse>>();
	}
}
