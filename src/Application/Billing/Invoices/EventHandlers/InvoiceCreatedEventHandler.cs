using Domain.Billing.Events;
using Microsoft.Extensions.Logging;
using SharedKernel.Interfaces.Messaging;

namespace Application.Billing.Invoices.EventHandlers;
internal class InvoiceCreatedEventHandler(ILogger<InvoiceCreatedEventHandler> logger)
	: IEventHandler<InvoiceCreatedDomainEvent>
{
	public Task Handle(InvoiceCreatedDomainEvent notification, CancellationToken cancellationToken)
	{
		logger.LogInformation("Invoice was created with id {Id}", notification.Invoice.Id);
		return Task.CompletedTask;
	}
}
