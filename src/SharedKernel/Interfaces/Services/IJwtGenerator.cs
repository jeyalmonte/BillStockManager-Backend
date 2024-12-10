namespace SharedKernel.Interfaces.Services;
public interface IJwtGenerator
{
	(string AccessToken, string RefreshToken) GenerateTokens(
		string userId,
		string userName,
		string email,
		IEnumerable<string> roles
		);
}
