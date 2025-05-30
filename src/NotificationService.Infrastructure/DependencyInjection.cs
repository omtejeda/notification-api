using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using NotificationService.Application.Contracts.Persistence;
using NotificationService.Application.Features.Providers.Interfaces;
using NotificationService.Infrastructure.Interfaces;
using NotificationService.Infrastructure.Providers;
using NotificationService.Infrastructure.Repositories;
using NotificationService.Infrastructure.Repositories.Helpers;
using NotificationService.Infrastructure.Services;

namespace NotificationService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IMongoDatabase>(s =>
            new MongoClient(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"))
                .GetDatabase(Environment.GetEnvironmentVariable("DB_NAME"))
                .InitializeMappings()
        );
                
        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

        services.AddTransient<IEmailProvider, SendGridProvider>();
        services.AddTransient<IEmailProvider, SmtpProvider>();
        services.AddTransient<IHttpClientProvider, HttpClientProvider>();
        services.AddTransient<IFirebaseService, FirebaseService>();
        services.AddTransient<IFirebaseProvider, FirebaseProvider>();
        return services;
    }
}