using Domain.Inventory.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Inventory.Products.EventHandlers;
public class ProductUpdatedEventHandler(ILogger<ProductUpdatedEventHandler> logger)
	: INotificationHandler<ProductUpdatedDomainEvent>
{
	public Task Handle(ProductUpdatedDomainEvent notification, CancellationToken cancellationToken)
	{
		logger.LogInformation("Product with '{Id}' was updated.", notification.Product.Id);
		return Task.CompletedTask;
	}
}
