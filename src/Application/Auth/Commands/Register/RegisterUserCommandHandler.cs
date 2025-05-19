using Application.Auth.Interfaces;
using Application.Auth.Models;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Auth.Commands.Register;
public class RegisterUserCommandHandler(IAuthService _identityService) : ICommandHandler<RegisterUserCommand, Success>
{
	public async Task<Result<Success>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
	{
		var userRegisterRequest = new UserRegisterRequest(
			name: request.Name,
			username: request.Username,
			email: request.Email,
			password: request.Password);

		var result = await _identityService.Register(userRegisterRequest);

		return result;
	}
}
