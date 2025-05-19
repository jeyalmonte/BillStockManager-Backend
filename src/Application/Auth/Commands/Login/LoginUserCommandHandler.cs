using Application.Auth.Interfaces;
using Application.Auth.Models;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Auth.Commands.Login;
public class LoginUserCommandHandler(IAuthService _identityService) : ICommandHandler<LoginUserCommand, UserTokenResponse>
{
	public async Task<Result<UserTokenResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
	{
		var userRequest = new UserRequest(
			username: request.Username,
			password: request.Password);

		var result = await _identityService.Login(userRequest);

		return result;
	}
}
