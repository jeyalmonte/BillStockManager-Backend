namespace SharedKernel.Interfaces;
public interface IUnitOfWork
{
	/// <summary>
	/// Confirms all changes made in the current transaction asynchronously.
	/// </summary>
	/// <param name="cancellationToken">Token to cancel the operation.</param>
	/// <returns>The number of affected rows.</returns>
	Task<int> CommitAsync(CancellationToken cancellationToken = default);
}
