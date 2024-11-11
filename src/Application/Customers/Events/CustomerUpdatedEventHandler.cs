using Domain.Customers.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Customers.Events;
internal class CustomerUpdatedEventHandler(ILogger<CustomerUpdatedEventHandler> logger) : INotificationHandler<CustomerUpdatedDomainEvent>
{
	public async Task Handle(CustomerUpdatedDomainEvent notification, CancellationToken cancellationToken)
	{
		await Task.CompletedTask;

		logger.LogInformation("Customer with id '{Id} was updated.'", notification.CustomerId);
	}
}
