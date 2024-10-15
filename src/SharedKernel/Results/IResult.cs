namespace SharedKernel.Results;

public interface IResult
{
	List<Error>? Errors { get; }
	bool HasError { get; }
}
public interface IResult<out TValue> : IResult
{
	TValue Value { get; }
}
