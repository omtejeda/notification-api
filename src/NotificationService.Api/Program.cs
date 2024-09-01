using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Api.Middlewares;
using NotificationService.Api.IoC;
using NotificationService.Core.IoC;
using NotificationService.Infrastructure.IoC;
using NotificationService.Common.Interfaces;
using NotificationService.Common.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IDateTimeService, DateTimeService>();
builder.Services.AddSingleton<IEnvironmentService, EnvironmentService>();

builder.Services.AddInfrastructure();
builder.Services.AddCore();
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

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseCors("NonProductionPolicy");

app.UseAuthorization();

app.UseMiddleware<AuthMiddleware>();

app.UseEndpoints(x =>
{
    x.MapControllers();
});

app.Run();