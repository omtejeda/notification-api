using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Api.Middlewares;
using NotificationService.Api.IoC;
using NotificationService.Application.IoC;
using NotificationService.Infrastructure.IoC;
using NotificationService.Common.Interfaces;
using NotificationService.Common.Services;
using NotificationService.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IDateTimeService, DateTimeService>();
builder.Services.AddSingleton<IEnvironmentService, EnvironmentService>();

builder.Services.AddInfrastructure();
builder.Services.AddApplication();
builder.Services.AddApi();

var app = builder.Build();
var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.RoutePrefix = string.Empty;

    foreach (var description in provider.ApiVersionDescriptions)
    {
        c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
    }
});

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