using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Identity.Services;

internal class JwtGeneratorService(
	IDateTimeProvider dateTimeProvider,
	IOptions<JwtSettings> _jwtSettings,
	UserManager<User> userManager
	) : IJwtGenerator
{
	public async Task<(string AccessToken, string RefreshToken)> GenerateTokens(User user)
	{
		var accessToken = await GenerateAccessToken(user);
		var refreshToken = GenerateRefreshToken();

		user.UpdateRefreshToken(refreshToken, dateTimeProvider.UtcNow.AddDays(7));

		await userManager.UpdateAsync(user);

		return (accessToken, refreshToken);
	}

	private async Task<string> GenerateAccessToken(User user)
	{
		var tokenHandler = new JwtSecurityTokenHandler();

		var key = Encoding.ASCII.GetBytes(_jwtSettings.Value.Secret);

		var claims = new List<Claim>
		{
			new(ClaimTypes.NameIdentifier, user.Id),
			new(ClaimTypes.Name, user.UserName!),
			new(ClaimTypes.Email, user.Email!)
		};

		var roles = await userManager.GetRolesAsync(user);

		claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));


		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(claims),
			Expires = dateTimeProvider.UtcNow.AddDays(1),
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
		};

		var token = tokenHandler.CreateToken(tokenDescriptor);

		return tokenHandler.WriteToken(token);
	}

	private static string GenerateRefreshToken()
	{
		var randomNumber = new byte[32];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomNumber);
		return Convert.ToBase64String(randomNumber);
	}
}
