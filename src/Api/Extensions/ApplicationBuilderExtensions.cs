using Infrastructure.Common.Persistence;

namespace Api.Extensions;

public static class ApplicationBuilderExtensions
{
	public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, IWebHostEnvironment env)
	{
		if (env.IsDevelopment())
		{
			app
				.UseSwagger()
				.UseSwaggerUI();
		}

		return app;
	}

	public static void InitializeDb(this IApplicationBuilder app)
	{
		using var serviceScope = app.ApplicationServices.CreateScope();

		var initializer = serviceScope.ServiceProvider.GetRequiredService<IAppDbInitializer>();

		var environment = serviceScope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

		Task.Run(async () =>
		{
			await initializer.InitializeAsync();

			if (environment.IsDevelopment())
			{
				await initializer.SeedAsync();
			}
		}).Wait();

	}
}
