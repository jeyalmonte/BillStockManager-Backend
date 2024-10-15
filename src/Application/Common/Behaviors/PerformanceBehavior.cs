using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Application.Common.Behaviors;

public class PerformanceBehavior<TRequest, TResponse>(ILogger<TRequest> _logger)
	: IPipelineBehavior<TRequest, TResponse>
	where TRequest : notnull
{
	private readonly Stopwatch _timer = new();

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		_timer.Start();

		var response = await next();

		_timer.Stop();

		var elapsedMilliseconds = _timer.ElapsedMilliseconds;

		if (elapsedMilliseconds > 500)
		{
			_logger.LogWarning("Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds)", typeof(TRequest).Name, elapsedMilliseconds);
		}

		return response;
	}
}
