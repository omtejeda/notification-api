using Microsoft.Extensions.DependencyInjection;
using NotificationService.Core.Contracts.Interfaces.Services;
using NotificationService.Core.Providers.Decorators;
using NotificationService.Core.Providers.Factories;
using NotificationService.Core.Providers.Factories.Interfaces;
using NotificationService.Core.Providers.Interfaces;
using NotificationService.Core.Providers.Services;

namespace NotificationService.Core.Providers.IoC
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