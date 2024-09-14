using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Application.Features.Providers.Decorators;
using NotificationService.Application.Providers.Factories;
using NotificationService.Application.Providers.Factories.Interfaces;
using NotificationService.Application.Providers.Interfaces;
using NotificationService.Application.Providers.Services;

namespace NotificationService.Application.Providers.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProviders(this IServiceCollection services)
        {
            services.AddTransient<IProviderService, ProviderService>();
            services.AddTransient<IEmailProviderFactory, EmailProviderFactory>();

            // Concrete providers services must be registered before this one
            services.Decorate<IEmailProvider, EmailProviderRetryDecorator>();
            
            return services;
        }
    }
}