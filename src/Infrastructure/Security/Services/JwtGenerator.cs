using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Security.Services;

internal class JwtGenerator(IOptions<JwtSettings> _jwtSettings) : IJwtGenerator
{
	public (string AccessToken, string RefreshToken) GenerateTokens(string userId, string userName, string email, IEnumerable<string> roles)
	{
		string accessToken = GenerateAccessToken(userId, userName, email, roles);

		string refreshToken = GenerateRefreshToken();

		return (accessToken, refreshToken);
	}

	private string GenerateAccessToken(string userId, string userName, string email, IEnumerable<string> roles)
	{
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.Secret));
		var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var claims = new List<Claim>
		{
			new(JwtRegisteredClaimNames.NameId,userId),
			new(JwtRegisteredClaimNames.UniqueName, userName),
			new(JwtRegisteredClaimNames.Email, email)
		};

		claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

		var jsonWebToken = new JwtSecurityToken(
			issuer: _jwtSettings.Value.Issuer,
			audience: _jwtSettings.Value.Audience,
			claims: claims,
			expires: DateTime.UtcNow.AddDays(_jwtSettings.Value.TokenExpirationInDays),
			signingCredentials: credentials
			);

		var tokenHandler = new JwtSecurityTokenHandler();

		return tokenHandler.WriteToken(jsonWebToken);
	}

	private static string GenerateRefreshToken()
	{
		var randomNumber = new byte[32];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomNumber);
		return Convert.ToBase64String(randomNumber);
	}

}
