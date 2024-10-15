using Application.Common.Interfaces;
using Domain.Common;
using Infrastructure.Common.Persistence;
using Infrastructure.Common.Persistence.Interceptors;
using Infrastructure.Outbox;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Interfaces.Services;

namespace Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services
			.AddPersistence()
			.AddServices();

		return services;
	}

	public static IServiceCollection AddPersistence(this IServiceCollection services)
	{
		services.AddTransient<IUnitOfWork, UnitOfWork>();
		services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));

		services.AddScoped<InsertOutboxMessageInterceptor>();
		services.AddScoped<PublishDomainEventsInterceptor>();

		services.AddDbContext<IAppDbContext, AppDbContext>((serviceProvider, options) =>
		{
			var outboxInterceptor = serviceProvider.GetRequiredService<InsertOutboxMessageInterceptor>();
			var eventInterceptor = serviceProvider.GetRequiredService<PublishDomainEventsInterceptor>();

			options.UseInMemoryDatabase("PeopleDb").AddInterceptors(outboxInterceptor, eventInterceptor);
		});

		return services;
	}

	private static IServiceCollection AddServices(this IServiceCollection services)
	{
		services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
		services.AddScoped<IUserProvider, UserProvider>();

		return services;
	}
}
