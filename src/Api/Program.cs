using Api;
using Api.Extensions;
using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddApiServices()
	.AddApplicationServices()
	.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseSwagger(builder.Environment);
app.UseRouting();
app.UseSecurityHeaders();
app.UseAuthentication();
app.UseAuthorization();
app.MapHealthChecks();
app.MapControllers();
app.InitializeDb(builder.Environment);
app.Run();
