using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Identity.Commands.Register;
public record RegisterUserCommand(
	string Name,
	string Username,
	string Email,
	string Password
	) : ICommand<Success>;