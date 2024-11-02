using Application.Identity.Commands.Login;
using Application.Identity.Commands.Register;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Results;

namespace Api.Controllers;
[Route("api/[controller]")]
public class IdentityController : ApiController
{
	[HttpPost(nameof(Register))]
	[EndpointSummary("Register a new user")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
	{
		var result = await Sender.Send(command);

		return result.Match(
			_ => Ok(),
			Problem);
	}

	[HttpPost(nameof(Login))]
	[EndpointSummary("Login a user")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
	{
		var result = await Sender.Send(command);

		return result.Match(Ok, Problem);
	}
}
