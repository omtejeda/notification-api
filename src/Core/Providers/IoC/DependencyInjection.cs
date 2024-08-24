using Microsoft.Extensions.DependencyInjection;
using NotificationService.Contracts.Interfaces.Services;
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
            services.AddTransient<IEmailProvider, SendGridProvider>();
            services.AddTransient<IEmailProvider, SmtpProvider>();
            services.Decorate<IEmailProvider, EmailProviderRetryDecorator>();
            services.AddTransient<IHttpClientProvider, HttpClientProvider>();
            return services;
        }
    }
}