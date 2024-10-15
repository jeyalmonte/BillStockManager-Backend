using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using SharedKernel.Domain;

namespace Infrastructure.Outbox;

public sealed class InsertOutboxMessageInterceptor : SaveChangesInterceptor
{
	public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
		DbContextEventData eventData,
		InterceptionResult<int> result,
		CancellationToken cancellationToken = default)
	{
		if (eventData.Context is not null)
		{
			InsertMessages(eventData.Context);
		}

		return base.SavingChangesAsync(eventData, result, cancellationToken);
	}

	/// <summary>
	///  it uses the ChangeTracker to get the domain events from the entities
	///   and then creates an OutboxMessage for each domain event.
	/// </summary>
	/// <param name="context"></param>
	private static void InsertMessages(DbContext context)
	{
		var domainEvents = context
			.ChangeTracker.Entries<IEntity>()
			.SelectMany(x => x.Entity.PopDomainEvents())
			.ToList();

		foreach (var domainEvent in domainEvents)
		{
			var outboxMessage = OutboxMessage.Create(
				domainEvent.GetType().Name,
				JsonConvert.SerializeObject(
					domainEvent,
					new JsonSerializerSettings
					{
						TypeNameHandling = TypeNameHandling.All
					})
				);

			context.Add(outboxMessage);
		}

	}
}

