using Application.Identity.Interfaces;
using Application.Identity.Models;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Identity.Commands.Login;
public class LoginUserCommandHandler(IIdentityService _identityService) : ICommandHandler<LoginUserCommand, UserTokenResponse>
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
