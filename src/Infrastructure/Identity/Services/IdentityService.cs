using Application.Identity.Interfaces;
using Application.Identity.Models;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Results;

namespace Infrastructure.Identity.Services;
public class IdentityService(
	UserManager<User> _userManager,
	IJwtGenerator _jwtGenerator
	) : IIdentityService
{
	private const string UserRole = "User";
	public async Task<Result<UserTokenResponse>> Login(UserRequest request)
	{
		var user = await _userManager.FindByNameAsync(request.Username);

		if (user is null)
		{
			return Error.Failure(description: "Invalid username or password.");
		}

		var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

		if (!isPasswordValid)
		{
			return Error.Failure(description: "Invalid username or password.");
		}
		var (accessToken, refreshToken) = await _jwtGenerator.GenerateTokens(user);

		return new UserTokenResponse(accessToken, refreshToken);
	}

	public async Task<Result<Success>> Register(UserRegisterRequest request)
	{
		var user = User.Create(request.Name, request.Username, request.Email);

		var isEmailTaken = await _userManager.FindByEmailAsync(request.Email);

		if (isEmailTaken is not null)
		{
			return Error.Conflict(description: "Email is already taken.");
		}

		var isUsernameTaken = await _userManager.FindByNameAsync(request.Username);

		if (isUsernameTaken is not null)
		{
			return Error.Conflict(description: "Username is already taken.");
		}

		var result = await _userManager.CreateAsync(user, request.Password);

		if (!result.Succeeded)
		{
			var errorMessage = result.Errors.First()?.Description ?? "An error occurred registering the user.";
			return Error.Failure(description: errorMessage);
		}

		var roleResult = await _userManager.AddToRoleAsync(user, UserRole);

		if (!roleResult.Succeeded)
		{
			return Error.Failure(description: "User was created, but failed to assign role.");
		}

		return Result.Success;
	}
}
