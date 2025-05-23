﻿using Application.Auth.Commands.Login;
using Application.Auth.Interfaces;
using Application.Auth.Models;

namespace Application.UnitTests.Auth.Commands;
public class LoginUserCommandTests()
{
	private readonly Mock<IAuthService> _identityService = new();
	private readonly CancellationToken _cancellationToken = new();

	[Fact]
	public void LoginUserCommand_WhenUsernameIsEmpty_ShouldReturnValidationError()
	{
		// Arrange
		var command = CreateLoginUserCommand(includeUsername: false);
		var validator = new LoginUserCommandValidator();

		// Act
		var result = validator.Validate(command);

		// Assert
		result.IsValid.Should().BeFalse();
	}

	[Fact]
	public async Task LoginUserCommand_WhenLoginFails_ShouldReturnFailureError()
	{
		// Arrange
		var command = CreateLoginUserCommand();
		var response = (Result<UserTokenResponse>)Error.Failure();

		_identityService.Setup(x => x.Login(It.IsAny<UserRequest>()))
			.ReturnsAsync(response);

		var handler = new LoginUserCommandHandler(_identityService.Object);

		// Act
		var result = await handler.Handle(command, _cancellationToken);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.Single().Should().BeOneOf(Error.Failure());
	}

	[Fact]
	public async Task LoginUserCommand_ShouldReturnUserTokenResponse()
	{
		// Arrange
		var command = CreateLoginUserCommand();
		var userTokenResponse = new UserTokenResponse("test", "test");
		var response = (Result<UserTokenResponse>)userTokenResponse;

		_identityService.Setup(x => x.Login(It.IsAny<UserRequest>()))
			.ReturnsAsync(response);

		var handler = new LoginUserCommandHandler(_identityService.Object);

		// Act
		var result = await handler.Handle(command, _cancellationToken);

		// Assert
		result.HasError.Should().BeFalse();
		result.Value.Should().BeEquivalentTo(userTokenResponse);
	}

	private static LoginUserCommand CreateLoginUserCommand(bool includeUsername = true)
		=> new(
			Username: includeUsername ? "test" : default!,
			Password: "test");
}
