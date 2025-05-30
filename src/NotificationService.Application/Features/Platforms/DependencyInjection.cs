using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Features.Platforms.Services;
using NotificationService.Application.Contracts.Services;

namespace NotificationService.Application.Platforms;

public static class DependencyInjection
{
    public static IServiceCollection AddPlatforms(this IServiceCollection services)
    {
        services.AddTransient<IPlatformService, PlatformService>();
        return services;
    }
}