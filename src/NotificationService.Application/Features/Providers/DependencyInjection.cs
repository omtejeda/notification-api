using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Application.Features.Providers.Decorators;
using NotificationService.Application.Features.Providers.Factories;
using NotificationService.Application.Features.Providers.Interfaces;
using NotificationService.Application.Features.Providers.Services;

namespace NotificationService.Application.Features.Providers;

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