using Microsoft.AspNetCore.Http;
using SharedKernel.Interfaces.Services;
using System.Security.Claims;

namespace Infrastructure.Common.Services;

public class UserProvider(IHttpContextAccessor httpContext) : IUserProvider
{
    public string? UserId => httpContext?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    public string? UserName => httpContext?.HttpContext?.User?.Identity?.Name;
}
