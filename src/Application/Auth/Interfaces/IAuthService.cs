using Application.Auth.Models;
using SharedKernel.Results;

namespace Application.Auth.Interfaces;
public interface IAuthService
{
	Task<Result<Success>> Register(UserRegisterRequest request);
	Task<Result<UserTokenResponse>> Login(UserRequest request);
}
