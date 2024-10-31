using Application.Identity.Models;
using SharedKernel.Contracts.Users.Requests;
using SharedKernel.Results;

namespace Application.Identity.Interfaces;
public interface IIdentityService
{
	Task<Result<UserTokenResponse>> Login(User request);
	Task<Result<Success>> Register(RegisterRequest request);
}
