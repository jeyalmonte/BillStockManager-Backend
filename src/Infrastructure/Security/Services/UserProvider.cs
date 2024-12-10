using Microsoft.AspNetCore.Http;
using SharedKernel.Interfaces.Services;
using System.Security.Claims;

namespace Infrastructure.Security.Services;

public class UserProvider(IHttpContextAccessor httpContextAccessor) : IUserProvider
{
	public string? UserId => GetClaimValue(ClaimTypes.NameIdentifier);
	public string? UserName => GetClaimValue(ClaimTypes.Name);
	public string? Role => GetClaimValue(ClaimTypes.Role);

	private string? GetClaimValue(string claimType)
		=> httpContextAccessor?.HttpContext?.User.Claims
			.SingleOrDefault(c => c.Type == claimType)?.Value;
}
