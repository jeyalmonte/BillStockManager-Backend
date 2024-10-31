using Application.Common.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace Infrastructure.Common.Persistence;

public class AppDbContext(DbContextOptions options) : IdentityDbContext<User>(options), IAppDbContext
{
	public new DbSet<TEntity> Set<TEntity>() where TEntity : class, IEntity
		=> base.Set<TEntity>();

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return base.SaveChangesAsync(cancellationToken);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

		base.OnModelCreating(modelBuilder);
	}
}
