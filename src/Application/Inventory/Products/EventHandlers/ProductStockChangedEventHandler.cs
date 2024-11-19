using Domain.Inventory.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Inventory.Products.EventHandlers;
public class ProductStockChangedEventHandler(ILogger<ProductStockChangedEventHandler> logger)
	: INotificationHandler<ProductStockChangedDomainEvent>
{
	public Task Handle(ProductStockChangedDomainEvent notification, CancellationToken cancellationToken)
	{
		string change = notification.Quantity > 0 ? "increased" : "reduced";

		logger.LogInformation("Product stock has {Change} by {Quantity} units for product ID {Id}",
			change, Math.Abs(notification.Quantity), notification.Id);

		return Task.CompletedTask;
	}
}
