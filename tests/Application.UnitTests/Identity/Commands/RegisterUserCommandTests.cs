using Application.Identity.Commands.Register;
using Application.Identity.Interfaces;
using Application.Identity.Models;

namespace Application.UnitTests.Identity.Commands;
public class RegisterUserCommandTests
{
	private readonly Mock<IIdentityService> _identityService = new();
	private readonly CancellationToken _cancellationToken = new();

	[Fact]
	public void RegisterUserCommand_WhenUsernameIsEmpty_ShouldReturnValidationError()
	{
		// Arrange
		var command = CreateRegisterUserCommand(includeUsername: false);
		var validator = new RegisterUserCommandValidator();

		// Act
		var result = validator.Validate(command);

		// Assert
		result.IsValid.Should().BeFalse();
	}

	[Fact]
	public async Task RegisterUserCommand_WhenRegisterFails_ShouldReturnError()
	{
		// Arrange 
		var command = CreateRegisterUserCommand();
		var response = (Result<Success>)Error.Conflict();

		_identityService.Setup(x => x.Register(It.IsAny<UserRegisterRequest>()))
			.ReturnsAsync(response);

		var handler = new RegisterUserCommandHandler(_identityService.Object);

		// Act
		var result = await handler.Handle(command, _cancellationToken);

		//Assert
		result.HasError.Should().BeTrue();
	}

	[Fact]
	public async Task RegisterUserCommand_ShouldRegisterUser()
	{
		// Arrange 
		var command = CreateRegisterUserCommand();
		var response = (Result<Success>)Result.Success;

		_identityService.Setup(x => x.Register(It.IsAny<UserRegisterRequest>()))
			.ReturnsAsync(response);

		var handler = new RegisterUserCommandHandler(_identityService.Object);

		// Act
		var result = await handler.Handle(command, _cancellationToken);

		//Assert
		result.HasError.Should().BeFalse();
	}

	private static RegisterUserCommand CreateRegisterUserCommand(bool includeUsername = true)
	   => new(
		   Name: "test",
		   Username: includeUsername ? "test" : default!,
		   Email: "tes",
		   Password: "test");
}

