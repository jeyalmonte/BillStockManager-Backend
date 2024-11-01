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
}
