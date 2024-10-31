using Application.Common.Interfaces;
using Application.Identity.Interfaces;
using Infrastructure.Common.Persistence;
using Infrastructure.Common.Persistence.Interceptors;
using Infrastructure.Common.Services;
using Infrastructure.Identity;
using Infrastructure.Identity.Configuration;
using Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Interfaces;
using SharedKernel.Interfaces.Services;

namespace Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services
			.AddPersistence(configuration)
			.AddServices()
			.AddIdentity(configuration);

		return services;
	}

	public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddTransient<IUnitOfWork, UnitOfWork>();

		services.AddSingleton<UpdateAuditableInterceptor>();
		services.AddSingleton<PublishDomainEventsInterceptor>();

		services.AddDbContext<IAppDbContext, AppDbContext>(
			(sp, options) => options
				.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
				.AddInterceptors(
					sp.GetRequiredService<UpdateAuditableInterceptor>(),
					sp.GetRequiredService<PublishDomainEventsInterceptor>()));

		return services;
	}

	private static IServiceCollection AddServices(this IServiceCollection services)
	{
		services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
		services.AddScoped<IUserProvider, UserProvider>();

		return services;
	}

	private static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
	{
		services
		   .AddTransient<IIdentityService, IdentityService>()
		   .AddTransient<IJwtGenerator, JwtGeneratorService>()
		   .AddIdentity<User, IdentityRole>(options =>
		   {
			   options.Password.RequiredLength = 8;
			   options.Password.RequireDigit = false;
			   options.Password.RequireLowercase = false;
			   options.Password.RequireNonAlphanumeric = false;
			   options.Password.RequireUppercase = false;
		   })
		   .AddEntityFrameworkStores<AppDbContext>();

		services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.Section));

		services
			.ConfigureOptions<JwtBearerTokenValidationConfiguration>()
			.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer();

		return services;
	}
}
