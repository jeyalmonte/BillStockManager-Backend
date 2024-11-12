using Domain.Inventory.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Inventory.Categories.EventHandlers;
public class CategoryCreatedEventHandler(ILogger<CategoryCreatedEventHandler> logger)
    : INotificationHandler<CategoryCreatedDomainEvent>
{
    public async Task Handle(CategoryCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        logger.LogInformation("Category with id '{Id}' was created.", notification.Category.Id);
    }
}
