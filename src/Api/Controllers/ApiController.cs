using Api.Utils;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SharedKernel.Results;

namespace Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
public class ApiController : ControllerBase
{
	protected const string Id = "{id}";

	private ISender? _sender;
	protected ISender Sender
		=> _sender ??= this.HttpContext
			.RequestServices
			.GetRequiredService<ISender>();

	protected ActionResult Problem(List<Error> errors)
	{
		if (errors.Count is 0)
		{
			return Problem();
		}

		if (errors.TrueForAll(error => error.Type == ErrorType.Validation))
		{
			return ValidationProblem(errors);
		}

		return Problem(errors[0]);
	}

	private ObjectResult Problem(Error error)
	{
		var statusCode = error.Type switch
		{
			ErrorType.Failure => StatusCodes.Status400BadRequest,
			ErrorType.Conflict => StatusCodes.Status409Conflict,
			ErrorType.Validation => StatusCodes.Status400BadRequest,
			ErrorType.NotFound => StatusCodes.Status404NotFound,
			_ => StatusCodes.Status500InternalServerError,
		};

		return Problem(statusCode: statusCode, title: error.Description);
	}

	private ActionResult ValidationProblem(List<Error> errors)
	{
		var modelStateDictionary = new ModelStateDictionary();

		errors.ForEach(error => modelStateDictionary.AddModelError(error.Code, error.Description));

		return ValidationProblem(modelStateDictionary);
	}
}
