using Domain.Inventory.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Inventory.Products.EventHandlers;
internal class ProductRemovedEventHandler(ILogger<ProductCreatedEventHandler> logger)
	: INotificationHandler<ProductRemovedDomainEvent>
{
	public Task Handle(ProductRemovedDomainEvent notification, CancellationToken cancellationToken)
	{
		logger.LogInformation("Product removed with ID {ProductId}", notification.Id);
		return Task.CompletedTask;
	}
}
