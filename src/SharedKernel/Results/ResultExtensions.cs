namespace SharedKernel.Results;

public static class ResultExtensions
{
	public static TResult Match<TValue, TResult>(this Result<TValue> result, Func<TValue, TResult> onValue, Func<List<Error>, TResult> onError)
	{
		if (result.HasError)
		{
			return onError(result.Errors);
		}
		return onValue(result.Value);
	}
}
