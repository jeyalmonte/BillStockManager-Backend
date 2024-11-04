using Application.Identity.Commands.Login;
using Application.Identity.Interfaces;
using Application.Identity.Models;

namespace Application.UnitTests.Identity.Commands;
public class LoginUserCommandTests()
{
    private readonly Mock<IIdentityService> _identityService = new();
    private readonly CancellationToken _cancellationToken = new();

    [Fact]
    public void LoginUserCommand_ShouldReturnValidationError_WhenUsernameIsEmpty()
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
    public async Task LoginUserCommand_ShouldReturnFailureError_WhenLoginFails()
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

    private LoginUserCommand CreateLoginUserCommand(bool includeUsername = true)
        => new(
            Username: includeUsername ? "test" : default!,
            Password: "test");
}
