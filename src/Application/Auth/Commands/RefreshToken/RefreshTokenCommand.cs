using Application.Auth.Models;
using SharedKernel.Interfaces.Messaging;

namespace Application.Auth.Commands.RefreshToken;
public record RefreshTokenCommand(
	string Token
	) : ICommand<UserTokenResponse>;
