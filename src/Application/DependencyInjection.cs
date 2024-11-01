using Application.Common.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

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

		return services;
	}

}
