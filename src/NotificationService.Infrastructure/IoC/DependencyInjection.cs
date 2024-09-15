using System;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using NotificationService.Application.Contracts.Interfaces.Repositories;
using NotificationService.Application.Features.Providers.Interfaces;
using NotificationService.Infrastructure.Providers;
using NotificationService.Infrastructure.Repositories;
using NotificationService.Infrastructure.Repositories.Helpers;

namespace NotificationService.Infrastructure.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IMongoDatabase>(s =>
                new MongoClient(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"))
                    .GetDatabase(Environment.GetEnvironmentVariable("DB_NAME"))
                    .InitializeMappings()
            );
            services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));

            services.AddTransient<IEmailProvider, SendGridProvider>();
            services.AddTransient<IEmailProvider, SmtpProvider>();
            services.AddTransient<IHttpClientProvider, HttpClientProvider>();
            return services;
        }
    }
}