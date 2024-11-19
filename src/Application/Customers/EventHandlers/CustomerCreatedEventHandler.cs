using Domain.Customers.Events;
using Microsoft.Extensions.Logging;
using SharedKernel.Interfaces.Messaging;

namespace Application.Customers.EventHandlers;
internal class CustomerCreatedEventHandler(ILogger<CustomerCreatedEventHandler> logger)
	: IEventHandler<CustomerCreatedDomainEvent>
{
	public async Task Handle(CustomerCreatedDomainEvent notification, CancellationToken cancellationToken)
	{
		await Task.CompletedTask;

		logger.LogInformation("Customer with id '{Id} was created.'", notification.Customer.Id);
	}
}
