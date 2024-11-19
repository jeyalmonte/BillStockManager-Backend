using Domain.Inventory.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Inventory.Products.EventHandlers;
public class ProductStockReducedEventHandler(ILogger<ProductStockReducedEventHandler> logger)
	: INotificationHandler<ProductStockReducedDomainEvent>
{
	public Task Handle(ProductStockReducedDomainEvent notification, CancellationToken cancellationToken)
	{
		logger.LogInformation(
			"Product stock reduced for product with ID {ProductId} by {Quantity}.",
			notification.ProductId,
			notification.Quantity);

		return Task.CompletedTask;
	}
}
