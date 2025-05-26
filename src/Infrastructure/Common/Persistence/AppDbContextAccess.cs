using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace Infrastructure.Common.Persistence;
/// <summary>
/// Abstract base class for accessing and managing a specific entity type in the database.
/// </summary>
/// <remarks>
/// Intended for inheritance; provides access to tracked and untracked DbSet<TEntity> queries.
/// </remarks>
/// <typeparam name="TEntity">Entity type, must inherit from <see cref="Entity"/>.</typeparam>

public abstract class AppDbContextAccess<TEntity>(AppDbContext dbContext) where TEntity : Entity
{
	/// <summary>
	/// Represents the database context used for interacting with the application's data store.
	/// </summary>
	/// <remarks>This field is intended for use within derived classes to perform database operations. It is
	/// initialized with an instance of <see cref="AppDbContext"/> and should not be modified.</remarks>
	protected readonly AppDbContext _dbContext = dbContext;

	/// <summary>
	/// Gets the <see cref="DbSet{TEntity}"/> instance for the current entity type.
	/// </summary>
	/// <remarks>This property provides access to the underlying database set for the specified entity type.  It can
	/// be used to query, add, update, or delete entities within the context of the current database session.</remarks>
	protected DbSet<TEntity> Entities => _dbContext.Set<TEntity>();

	/// <summary>
	/// Gets a queryable collection of entities with no tracking enabled.
	/// </summary>
	/// <remarks>Use this property when you need to retrieve entities for read-only purposes or when tracking
	/// changes is not required. This can improve performance by avoiding the overhead of change tracking.</remarks>
	protected IQueryable<TEntity> EntitiesAsNoTracking => Entities.AsNoTracking();
}
