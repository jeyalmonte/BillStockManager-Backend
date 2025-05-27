using Application.Auth.Interfaces;
using Application.Auth.Models;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Auth.Commands.Register;
public class RegisterUserCommandHandler(IAuthService authService) : ICommandHandler<RegisterUserCommand, Success>
{
	private readonly IAuthService _authService = authService;
	public async Task<Result<Success>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
	{
		var userRegisterRequest = new UserRegisterRequest(
			name: request.Name,
			username: request.Username,
			email: request.Email,
			password: request.Password);

		var result = await _authService.Register(userRegisterRequest);

		return result;
	}
}
