using Application.Identity.Interfaces;
using Application.Identity.Models;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Contracts.Users.Requests;
using SharedKernel.Contracts.Users.Responses;
using SharedKernel.Results;

namespace Infrastructure.Identity.Services;
public class IdentityService(
	UserManager<User> _userManager,
	IJwtGenerator _jwtGenerator
	) : IIdentityService
{
	public async Task<Result<LoginResponse>> Login(LoginRequest request)
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

		return new LoginResponse(accessToken, refreshToken);
	}

	public async Task<Result<Success>> Register(RegisterRequest request)
	{
		var user = User.Create(request.Name, request.Username, request.Email);

		var isEmailTaken = await _userManager.FindByEmailAsync(request.Email);

		if (isEmailTaken is not null)
		{
			return Error.Failure(description: "Email is already taken.");
		}

		var isUsernameTaken = await _userManager.FindByNameAsync(request.Username);

		if (isUsernameTaken is not null)
		{
			return Error.Failure(description: "Username is already taken.");
		}

		var result = await _userManager.CreateAsync(user, request.Password);

		if (!result.Succeeded)
		{
			var errorMessage = result.Errors.First()?.Description ?? "An error occurred registering the user.";
			return Error.Failure(description: errorMessage);
		}

		return Result.Success;
	}
}
