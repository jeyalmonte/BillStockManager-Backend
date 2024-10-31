namespace Application.Identity.Models;
public class UserTokenResponse(string accessToken, string refreshToken)
{
	public string AccessToken { get; } = accessToken;
	public string RefreshToken { get; } = refreshToken;
}
