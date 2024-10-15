using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SharedKernel.Domain;

namespace Infrastructure.Common.Persistence.Interceptors;

public sealed class PublishDomainEventsInterceptor(IPublisher _publisher) : SaveChangesInterceptor
{
	public override async ValueTask<int> SavedChangesAsync(
		SaveChangesCompletedEventData eventData,
		int result,
		CancellationToken cancellationToken = default)
	{
		if (eventData.Context is not null)
		{
			await PublishDomainEvents(eventData.Context, cancellationToken);
		}

		return await base.SavedChangesAsync(eventData, result, cancellationToken);
	}

	private async Task PublishDomainEvents(DbContext context, CancellationToken cancellationToken)
	{
		var domainEvents = context
			.ChangeTracker.Entries<IEntity>()
			.SelectMany(x => x.Entity.PopDomainEvents())
			.ToList();

		foreach (var domainEvent in domainEvents)
		{
			await _publisher.Publish(domainEvent, cancellationToken);
		}
	}
}
