using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Api.Extensions;

public static class EndpointRouteBuilderExtensions
{
	public static IEndpointRouteBuilder MapHealthChecks(
		this IEndpointRouteBuilder endpoints)
	{
		endpoints.MapHealthChecks("/health", new HealthCheckOptions
		{
			ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
		});

		return endpoints;
	}
}
