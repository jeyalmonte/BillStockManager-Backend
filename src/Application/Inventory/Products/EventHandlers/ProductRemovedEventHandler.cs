using Domain.Inventory.Events;
using Microsoft.Extensions.Logging;
using SharedKernel.Interfaces.Messaging;

namespace Application.Inventory.Products.EventHandlers;
internal class ProductRemovedEventHandler(ILogger<ProductRemovedEventHandler> logger)
	: IEventHandler<ProductRemovedDomainEvent>
{
	public Task Handle(ProductRemovedDomainEvent notification, CancellationToken cancellationToken)
	{
		logger.LogInformation("Product removed with ID {ProductId}", notification.Id);
		return Task.CompletedTask;
	}
}
