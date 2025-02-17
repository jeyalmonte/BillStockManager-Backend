using Domain.Billing.Events;
using Domain.Billing.Repositories;
using SharedKernel.Interfaces;
using SharedKernel.Interfaces.Messaging;

namespace Application.Billing.Invoices.EventHandlers;
internal class InvoicePaidEventHandler(
	IInvoiceRepository invoiceRepository,
	IUnitOfWork unitOfWork
	) : IEventHandler<InvoicePaidDomainEvent>
{
	public async Task Handle(InvoicePaidDomainEvent notification, CancellationToken cancellationToken)
	{
		var invoice = await invoiceRepository.GetByIdAsync(
			id: notification.InvoiceId,
			asNoTracking: false,
			cancellationToken: cancellationToken) ?? throw new InvalidOperationException("Invoice not found.");

		invoice.MarkAsPaid();

		await unitOfWork.CommitAsync(cancellationToken);
	}
}
