using Domain.Inventory.Events;
using Microsoft.Extensions.Logging;
using SharedKernel.Interfaces.Messaging;

namespace Application.Inventory.Products.EventHandlers;
internal class ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger)
	: IEventHandler<ProductCreatedDomainEvent>
{
	public Task Handle(ProductCreatedDomainEvent notification, CancellationToken cancellationToken)
	{
		logger.LogInformation("Product with '{Id}' was created.", notification.Id);
		return Task.CompletedTask;
	}
}
