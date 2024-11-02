using Application.Identity.Models;
using SharedKernel.Interfaces.Messaging;

namespace Application.Identity.Commands.Login;
public record LoginUserCommand(
	string Username,
	string Password
	) : ICommand<UserTokenResponse>;
