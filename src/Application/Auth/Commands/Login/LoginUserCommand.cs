using Application.Auth.Models;
using SharedKernel.Interfaces.Messaging;

namespace Application.Auth.Commands.Login;
public record LoginUserCommand(
	string Username,
	string Password
	) : ICommand<UserTokenResponse>;
