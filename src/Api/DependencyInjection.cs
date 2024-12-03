using Api.Extensions;
using Api.Infrastructure;
using Api.Utils;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace Api;

public static class DependencyInjection
{
	public static IServiceCollection AddApiServices(this IServiceCollection services)
	{
		services
			.AddRoutingAndControllers()
			.AddApiVersioning()
			.AddSwagger()
			.AddExceptionHandling()
			.AddHttpContextAccessor();

		return services;
	}

	private static IServiceCollection AddRoutingAndControllers(this IServiceCollection services)
	{
		services
			.AddRouting(options => options.LowercaseUrls = true);

		services
			.AddControllers(x =>
				x.UseGeneralRoutePrefix(ApiRoute.GlobalPrefix))
			.ConfigureApiBehaviorOptions(options =>
			{
				options.SuppressModelStateInvalidFilter = true;
			})
			.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

		services
			.AddEndpointsApiExplorer();

		return services;
	}

	private static IServiceCollection AddApiVersioning(this IServiceCollection services)
	{
		services.AddApiVersioning(opts =>
		{
			opts.ReportApiVersions = true;
			opts.AssumeDefaultVersionWhenUnspecified = true;
			opts.DefaultApiVersion = new ApiVersion(1, 0);
			opts.ApiVersionReader = ApiVersionReader.Combine(
				new UrlSegmentApiVersionReader(),
				new HeaderApiVersionReader("x-api-version"));
		}).AddApiExplorer(opts =>
		{
			opts.GroupNameFormat = "'v'VVV";
			opts.SubstituteApiVersionInUrl = true;
		});

		return services;
	}

	private static IServiceCollection AddSwagger(this IServiceCollection services)
	{
		var apiVersionProvider = services
			.BuildServiceProvider()
			.GetRequiredService<IApiVersionDescriptionProvider>();

		services.AddSwaggerGen(c =>
		{
			c.EnableAnnotations();
			foreach (var description in apiVersionProvider.ApiVersionDescriptions)
			{

				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "BillStockManager API",
					Description = "Bill Stock Manager",
					Version = description.ApiVersion.ToString(),
					Contact = new OpenApiContact
					{
						Name = "Software Developer | Jeyson Almonte",
						Email = "Jeysom28@gmail.com",
					},
				});
			}
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
