using Domain.Inventory.Events;
using Microsoft.Extensions.Logging;
using SharedKernel.Interfaces.Messaging;

namespace Application.Inventory.Categories.EventHandlers;
internal class CategoryCreatedEventHandler(ILogger<CategoryCreatedEventHandler> logger)
	: IEventHandler<CategoryCreatedDomainEvent>
{
	public async Task Handle(CategoryCreatedDomainEvent notification, CancellationToken cancellationToken)
	{
		await Task.CompletedTask;
		logger.LogInformation("Category with id '{Id}' was created.", notification.Category.Id);
	}
}
