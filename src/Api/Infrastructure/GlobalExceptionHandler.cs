using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.Infrastructure;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> _logger) : IExceptionHandler
{
	public async ValueTask<bool> TryHandleAsync(
		HttpContext httpContext,
		Exception exception,
		CancellationToken cancellationToken)
	{
		_logger.LogError(exception, "An Exception occurred: {Message}", exception.Message);

		var problemDetails = new ProblemDetails
		{
			Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.1",
			Title = "Server Error",
			Status = StatusCodes.Status500InternalServerError,
			Detail = exception.Message,
		};

		httpContext.Response.StatusCode = problemDetails.Status.Value;
		await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);

		return true;
	}
}
