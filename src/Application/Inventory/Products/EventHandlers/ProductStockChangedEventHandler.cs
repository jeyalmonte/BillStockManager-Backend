using Domain.Inventory.Events;
using Microsoft.Extensions.Logging;
using SharedKernel.Interfaces.Messaging;

namespace Application.Inventory.Products.EventHandlers;
internal class ProductStockChangedEventHandler(ILogger<ProductStockChangedEventHandler> logger)
	: IEventHandler<ProductStockChangedDomainEvent>
{
	public Task Handle(ProductStockChangedDomainEvent notification, CancellationToken cancellationToken)
	{
		string change = notification.Quantity > 0 ? "increased" : "reduced";

		logger.LogInformation("Product stock has {Change} by {Quantity} units for product ID {Id}",
			change, Math.Abs(notification.Quantity), notification.Id);

		return Task.CompletedTask;
	}
}
