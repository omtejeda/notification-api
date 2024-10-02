using NotificationService.Api.Middlewares;
using NotificationService.Application;
using NotificationService.Infrastructure;
using NotificationService.SharedKernel.Interfaces;
using NotificationService.Application.Common.Services;
using NotificationService.Api.Extensions;
using NotificationService.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IDateTimeService, DateTimeService>();
builder.Services.AddSingleton<IEnvironmentService, EnvironmentService>();

builder.Services.AddInfrastructure();
builder.Services.AddApplication();
builder.Services.AddApi();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseCustomSwaggerUI();

app.UseHttpsRedirection();

app.UseRouting();

app.UseLocalization(builder.Configuration);

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseCors("NonProductionPolicy");

app.UseAuthorization();

app.UseMiddleware<AuthMiddleware>();

app.UseEndpoints(x =>
{
    _ = x.MapControllers();
});

app.Run();