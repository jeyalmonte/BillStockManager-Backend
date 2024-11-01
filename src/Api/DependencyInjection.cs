using Api.Infrastructure;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace Api;

public static class DependencyInjection
{
	public static IServiceCollection AddApiServices(this IServiceCollection services)
	{
		services
			.AddRoutingAndControllers()
			.AddSwagger()
			.AddExceptionHandling()
			.AddHttpContextAccessor();

		return services;
	}

	private static IServiceCollection AddRoutingAndControllers(this IServiceCollection services)
	{
		services.AddRouting(options => options.LowercaseUrls = true)
				.AddControllers()
				.ConfigureApiBehaviorOptions(options =>
				{
					options.SuppressModelStateInvalidFilter = true;
				})
				.AddJsonOptions(options =>
				{
					options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
				});

		services.AddEndpointsApiExplorer();
		return services;
	}

	private static IServiceCollection AddSwagger(this IServiceCollection services)
	{
		services.AddSwaggerGen(c =>
		{
			c.EnableAnnotations();
			c.SwaggerDoc("v1", new OpenApiInfo
			{
				Title = "BillStockManager API",
				Description = "Bill Stock Manager",
				Version = "v1"
			});
		});
		return services;
	}

	private static IServiceCollection AddExceptionHandling(this IServiceCollection services)
	{
		services.AddExceptionHandler<GlobalExceptionHandler>();
		services.AddProblemDetails();
		return services;
	}
}
