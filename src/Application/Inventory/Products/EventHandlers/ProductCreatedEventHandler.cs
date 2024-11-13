using Domain.Inventory.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Inventory.Products.EventHandlers;
public class ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger)
    : INotificationHandler<ProductCreatedDomainEvent>
{
    public Task Handle(ProductCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Product with '{Id}' was created.", notification.Id);
        return Task.CompletedTask;
    }
}
