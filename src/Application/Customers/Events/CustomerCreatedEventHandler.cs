using Domain.Customers.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Customers.Events;
public class CustomerCreatedEventHandler(ILogger<CustomerCreatedEventHandler> logger)
    : INotificationHandler<CustomerCreatedEvent>
{
    public async Task Handle(CustomerCreatedEvent notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        logger.LogInformation("Customer with id '{Id} was created.'", notification.Customer.Id);
    }
}
