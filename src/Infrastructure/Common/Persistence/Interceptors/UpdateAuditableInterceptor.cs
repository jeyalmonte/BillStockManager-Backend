using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SharedKernel.Domain;
using SharedKernel.Interfaces.Services;

namespace Infrastructure.Common.Persistence.Interceptors;
public class UpdateAuditableInterceptor(IDateTimeProvider dateTimeProvider) : SaveChangesInterceptor
{
	public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
	   DbContextEventData eventData,
	   InterceptionResult<int> result,
	   CancellationToken cancellationToken = default)
	{
		if (eventData.Context is not null)
		{
			UpdateAuditableEntities(eventData.Context);
		}

		return base.SavingChangesAsync(eventData, result, cancellationToken);
	}

	private void UpdateAuditableEntities(DbContext context)
	{
		var entries = context.ChangeTracker.Entries<BaseAuditableEntity>();

		foreach (var entry in entries)
		{
			switch (entry.State)
			{
				case EntityState.Added:
					entry.Entity.CreatedOn = dateTimeProvider.UtcNow;
					break;
				case EntityState.Modified:
					entry.Entity.UpdatedOn = dateTimeProvider.UtcNow;
					break;
			}
		}
	}
}
