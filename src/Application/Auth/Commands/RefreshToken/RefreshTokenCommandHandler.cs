using Application.Auth.Interfaces;
using Application.Auth.Models;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Auth.Commands.RefreshToken;
public class RefreshTokenCommandHandler(IAuthService _authService) : ICommandHandler<RefreshTokenCommand, UserTokenResponse>
{
	public async Task<Result<UserTokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
	{
		var tokenResponse = await _authService.RefreshToken(request.Token);

		if (tokenResponse.HasError)
		{
			return tokenResponse.Errors;
		}

		return tokenResponse;
	}
}
