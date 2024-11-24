using Application.Billing.Invoices.Services;
using Application.Billing.Invoices.Services.Interfaces;
using Application.Common.Behaviors;
using FluentValidation;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
		services.AddMediatR(options =>
		{
			options.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly);
			options.AddOpenBehavior(typeof(ValidationBehavior<,>));
			options.AddOpenBehavior(typeof(PerformanceBehavior<,>));
		});

		services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));

		var config = TypeAdapterConfig.GlobalSettings;
		config.Scan(Assembly.GetExecutingAssembly());

		services.AddSingleton(config);
		services.AddServices();

		return services;
	}

	private static IServiceCollection AddServices(this IServiceCollection services)
	{
		services.AddScoped<IInvoiceService, InvoiceService>();

		return services;
	}

}
