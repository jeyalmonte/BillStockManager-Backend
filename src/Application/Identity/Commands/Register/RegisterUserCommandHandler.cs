using Application.Identity.Interfaces;
using Application.Identity.Models;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Identity.Commands.Register;
public class RegisterUserCommandHandler(IIdentityService _identityService) : ICommandHandler<RegisterUserCommand, Success>
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
