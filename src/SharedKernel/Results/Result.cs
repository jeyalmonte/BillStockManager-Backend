namespace SharedKernel.Results;

public readonly record struct Result<TValue> : IResult<TValue>
{
	private readonly List<Error>? _errors;
	private readonly TValue? _value;
	private Result(TValue? value = default, List<Error>? errors = default, bool hasError = false)
	{
		_errors = errors;
		_value = value;
		HasError = hasError;
	}

	public TValue Value => _value!;
	public bool HasError { get; }
	public List<Error> Errors
	{
		get
		{
			if (!HasError)
			{
				return [Error.None];
			}

			return _errors!;
		}
	}

	public static implicit operator Result<TValue>(TValue value)
		=> new(value: value);

	public static implicit operator Result<TValue>(Error error)
		=> new(errors: [error], hasError: true);

	public static implicit operator Result<TValue>(List<Error> errors)
	   => new(errors: errors, hasError: true);

	public static implicit operator Result<TValue>(Error[] errors)
		=> new(errors: [.. errors], hasError: true);
}