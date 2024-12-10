using Application.Identity.Models;
using SharedKernel.Results;

namespace Application.Identity.Interfaces;
public interface IIdentityService
{
	Task<Result<Success>> Register(UserRegisterRequest request);
	Task<Result<UserTokenResponse>> Login(UserRequest request);
}
