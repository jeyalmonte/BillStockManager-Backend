using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure.Identity.Configuration;
public sealed class JwtBearerTokenValidationConfiguration(IOptions<JwtSettings> jwtSettings)
	: IConfigureNamedOptions<JwtBearerOptions>
{
	private readonly JwtSettings _jwtSettings = jwtSettings.Value;

	public void Configure(string? name, JwtBearerOptions options) => Configure(options);

	public void Configure(JwtBearerOptions options)
	{
		options.RequireHttpsMetadata = false;
		options.SaveToken = true;
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = _jwtSettings.Issuer,
			ValidAudience = _jwtSettings.Audience,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
		};
	}
}
