namespace SharedKernel.Interfaces.Services;

public interface IUserProvider
{
	public string? UserId { get; }
	public string? UserName { get; }
	public string? Role { get; }
}
