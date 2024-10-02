using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace NotificationService.Api.Extensions;

/// <summary>
/// Provides extension methods for configuring Swagger documentation in the application.
/// </summary>
public static class SwaggerExtensions
{
    /// <summary>
    /// Configures Swagger services for the application.
    /// This method sets up Swagger generation, including the API title, version, contact information,
    /// and security requirements for API key authentication.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add Swagger services to.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> with Swagger services added.</returns>
    public static IServiceCollection AddAppSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Notification API",
                Version = "v1",
                Contact = new OpenApiContact 
                { 
                    Name = "Software Engineer | Omarky Tejeda", 
                    Email = "omarkytejeda@gmail.com" 
                },
                Description = $"Environment: {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}"
            });

            c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
            {
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Name = "ApiKey"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKey" }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }

    public static IApplicationBuilder UseCustomSwaggerUI(this IApplicationBuilder app)
    {
        var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

        app.UseSwaggerUI(c =>
        {
            c.RoutePrefix = string.Empty;
            foreach (var description in provider.ApiVersionDescriptions)
            {
                c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
            }
        });

        return app;
    }
}