using Microsoft.OpenApi.Models;

namespace NotificationService.Api.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddAppSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen( c => 
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Notification API",
                Version = "v1",
                Contact = new OpenApiContact { Name = "Software Engineer | Omarky Tejeda", Email = "omarkytejeda@gmail.com" },
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
                    new string[] { }
                }
            });
        });
        
        return services;
    }
}