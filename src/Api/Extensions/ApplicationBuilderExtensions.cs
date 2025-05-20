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

	public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app)
	{
		app.Use((ctx, next) =>
		{
			ctx.Response.Headers.Append("X-Frame-Options", "DENY"); // Prevents the page from being embedded in a frame to avoid clickjacking attacks.
			ctx.Response.Headers.Append("X-Content-Type-Options", "nosniff"); // Prevents the browser from guessing the MIME type of files, ensuring they are interpreted correctly.
			ctx.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin"); // Controls the referrer information sent with requests to protect user privacy.
			ctx.Response.Headers.Append("Permissions-Policy", "geolocation=(self), microphone=(self), camera=(self)"); // Restricts access to sensitive features like geolocation, microphone, and camera to the same origin.

			return next();
		});

		// Enable HSTS only when using HTTPS in production
		// app.UseHsts();

		return app;
	}

	public static void InitializeDb(this IApplicationBuilder app, IWebHostEnvironment env)
	{
		using var serviceScope = app.ApplicationServices.CreateScope();

		var initializer = serviceScope.ServiceProvider.GetRequiredService<IAppDbInitializer>();

		Task.Run(async () =>
		{
			await initializer.InitializeAsync();

			if (env.IsDevelopment())
			{
				await initializer.SeedAsync();
			}
		}).Wait();

	}

}
