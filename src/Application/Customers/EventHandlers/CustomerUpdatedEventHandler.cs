using Domain.Customers.Events;
using Microsoft.Extensions.Logging;
using SharedKernel.Interfaces.Messaging;

namespace Application.Customers.EventHandlers;
internal class CustomerUpdatedEventHandler(ILogger<CustomerUpdatedEventHandler> logger)
	: IEventHandler<CustomerUpdatedDomainEvent>
{
	public async Task Handle(CustomerUpdatedDomainEvent notification, CancellationToken cancellationToken)
	{
		await Task.CompletedTask;

		logger.LogInformation("Customer with id '{Id} was updated.'", notification.CustomerId);
	}
}
