using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SharedKernel.Domain;
using SharedKernel.Interfaces.Services;

namespace Infrastructure.Common.Persistence.Interceptors;
public class UpdateAuditableInterceptor(IDateTimeProvider dateTimeProvider, IUserProvider userProvider) : SaveChangesInterceptor
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
		var entries = context.ChangeTracker.Entries<AuditableEntity>();

		foreach (var entry in entries)
		{
			switch (entry.State)
			{
				case EntityState.Added:
					entry.Entity.CreatedOn = dateTimeProvider.UtcNow;
					entry.Entity.CreatedBy = userProvider.UserName;
					break;
				case EntityState.Modified:
					entry.Entity.UpdatedOn = dateTimeProvider.UtcNow;
					entry.Entity.UpdatedBy = userProvider.UserName;
					break;
			}
		}
	}
}
