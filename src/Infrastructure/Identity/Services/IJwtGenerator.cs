namespace Infrastructure.Identity.Services;

public interface IJwtGenerator
{
	Task<(string AccessToken, string RefreshToken)> GenerateTokens(User user);
}
