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
app.UseAuthentication();
app.MapHealthChecks();
app.MapControllers();
app.InitializeDb();
app.Run();
