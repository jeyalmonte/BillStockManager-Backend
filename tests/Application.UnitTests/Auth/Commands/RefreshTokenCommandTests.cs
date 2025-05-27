using Application.Auth.Commands.RefreshToken;
using Application.Auth.Interfaces;
using Application.Auth.Models;

namespace Application.UnitTests.Auth.Commands;
public class RefreshTokenCommandTests
{
	private readonly Mock<IAuthService> _authServiceMock = new();

	[Fact]
	public async Task RefreshToken_WhenTokenIsInvalid_ReturnsError()
	{
		// Arrange
		var command = new RefreshTokenCommand("invalid_token");

		_authServiceMock.Setup(x => x.RefreshToken(command.Token))
			.ReturnsAsync(Error.Failure(description: "Invalid token."));

		var handler = new RefreshTokenCommandHandler(_authServiceMock.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.Single().Description.Should().Be("Invalid token.");
	}

	[Fact]
	public async Task RefreshToken_WhenTokenIsValid_ReturnsUserTokenResponse()
	{
		// Arrange
		var command = new RefreshTokenCommand("valid_token");
		var expectedResponse = new UserTokenResponse("new_access_token", "new_refresh_token");

		_authServiceMock.Setup(x => x.RefreshToken(command.Token))
			.ReturnsAsync(expectedResponse);

		var handler = new RefreshTokenCommandHandler(_authServiceMock.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.HasError.Should().BeFalse();
		result.Value.Should().BeEquivalentTo(expectedResponse);
	}
}
