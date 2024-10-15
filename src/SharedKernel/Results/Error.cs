namespace SharedKernel.Results;
public readonly record struct Error(
	string Code,
	string Description,
	ErrorType ErrorType)
{
	public string Code { get; } = Code;
	public string Description { get; } = Description;
	public ErrorType Type { get; } = ErrorType;
	internal static Error None => new(string.Empty, string.Empty, ErrorType.None);
	public static Error Failure(string code = "Failure", string description = "A 'Failure' error has occurred.")
		=> new(code, description, ErrorType.Failure);
	public static Error NotFound(string code = "Not Found", string description = "A 'NotFound' error has occurred.")
		=> new(code, description, ErrorType.NotFound);
	public static Error Validation(string code = "Validation", string description = "A 'Validation' error has occurred.")
		=> new(code, description, ErrorType.Validation);
	public static Error Conflict(string code = "Conflict", string description = "A 'Conflict' error has occurred.")
		=> new(code, description, ErrorType.Conflict);
	public static Error Custom(string code, string description, ErrorType errorType)
		=> new(code, description, errorType);

	public static implicit operator string(Error error)
		=> error.Code ?? string.Empty;
}

public enum ErrorType
{
	None,
	Failure,
	Validation,
	Unexpected,
	Conflict,
	NotFound,
	Unauthorized,
	Forbidden,
}

