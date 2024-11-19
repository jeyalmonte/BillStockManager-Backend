using Domain.Inventory.Events;
using Microsoft.Extensions.Logging;
using SharedKernel.Interfaces.Messaging;

namespace Application.Inventory.Products.EventHandlers;
internal class ProductUpdatedEventHandler(ILogger<ProductUpdatedEventHandler> logger)
	: IEventHandler<ProductUpdatedDomainEvent>
{
	public Task Handle(ProductUpdatedDomainEvent notification, CancellationToken cancellationToken)
	{
		logger.LogInformation("Product with '{Id}' was updated.", notification.Product.Id);
		return Task.CompletedTask;
	}
}
