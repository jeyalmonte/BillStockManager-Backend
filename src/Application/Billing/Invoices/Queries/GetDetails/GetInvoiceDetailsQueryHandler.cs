﻿using Domain.Billing.Repositories;
using Mapster;
using SharedKernel.Contracts.Invoices;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Billing.Invoices.Queries.GetDetails;
public class GetInvoiceDetailsQueryHandler(
	IInvoiceRepository invoiceRepository,
	IInvoiceDetailRepository invoiceDetailRepository)
	: IQueryHandler<GetInvoiceDetailsQuery, ICollection<InvoiceDetailResponse>>
{
	public async Task<Result<ICollection<InvoiceDetailResponse>>> Handle(GetInvoiceDetailsQuery request, CancellationToken cancellationToken)
	{
		var invoice = await invoiceRepository.GetByIdAsync(
			id: request.Id,
			cancellationToken: cancellationToken);

		if (invoice is null)
		{
			return Error.NotFound(description: "Invoice not found.");
		}

		var details = await invoiceDetailRepository.GetDetailsWithProductByInvoiceIdAsync(
			invoiceId: request.Id,
			cancellationToken: cancellationToken);

		return details.Adapt<List<InvoiceDetailResponse>>();
	}
}