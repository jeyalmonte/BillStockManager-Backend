﻿using Application.Auth.Interfaces;
using Application.Common.Interfaces;
using Domain.Billing.Repositories;
using Domain.Customers.Repositories;
using Domain.Inventory.Repositories;
using Infrastructure.Billing.Persistence.Repositories;
using Infrastructure.Common.Persistence;
using Infrastructure.Common.Persistence.Interceptors;
using Infrastructure.Common.Services;
using Infrastructure.Customers.Persistence;
using Infrastructure.Identity;
using Infrastructure.Identity.Services;
using Infrastructure.Inventory.Persistence.Repositories;
using Infrastructure.Security;
using Infrastructure.Security.Configuration;
using Infrastructure.Security.Services;
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
	public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
	{
		services
			.AddHttpContextAccessor()
			.AddPersistence(configuration)
			.AddIdentity()
			.AddServices()
			.AddAuthentication(configuration)
			.AddHealth();

		return services;
	}

	public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddTransient<IUnitOfWork, UnitOfWork>();

		services
			.AddScoped<UpdateAuditableInterceptor>()
			.AddScoped<PublishDomainEventsInterceptor>();

		services.AddDbContext<IAppDbContext, AppDbContext>(
			(sp, options) => options
				.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
				.AddInterceptors(
					sp.GetRequiredService<UpdateAuditableInterceptor>(),
					sp.GetRequiredService<PublishDomainEventsInterceptor>()))
				.AddTransient<IAppDbInitializer, AppDbInitializer>();

		services
			.AddScoped<ICustomerRepository, CustomerRepository>()
			.AddScoped<ICategoryRepository, CategoryRepository>()
			.AddScoped<IProductRepository, ProductRepository>()
			.AddScoped<IInvoiceRepository, InvoiceRepository>()
			.AddScoped<IInvoiceDetailRepository, InvoiceDetailRepository>();

		return services;
	}

	private static IServiceCollection AddIdentity(this IServiceCollection services)
	{
		services
		   .AddTransient<IAuthService, AuthService>()
		   .AddIdentity<User, IdentityRole>(options =>
		   {
			   options.Password.RequiredLength = 8;
			   options.Password.RequireDigit = false;
			   options.Password.RequireLowercase = false;
			   options.Password.RequireNonAlphanumeric = false;
			   options.Password.RequireUppercase = false;
		   })
		   .AddEntityFrameworkStores<AppDbContext>();

		return services;
	}

	private static IServiceCollection AddServices(this IServiceCollection services)
	{
		services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();

		return services;
	}

	private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.Section));

		services.AddSingleton<IJwtGenerator, JwtGenerator>();
		services.AddScoped<IUserProvider, UserProvider>();

		services
			.ConfigureOptions<JwtBearerTokenValidationConfiguration>()
			.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer();

		return services;
	}

	private static IServiceCollection AddHealth(this IServiceCollection services)
	{
		services
			.AddHealthChecks()
			.AddDbContextCheck<AppDbContext>();

		return services;
	}
}
