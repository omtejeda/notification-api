using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Platforms.Services;
using NotificationService.Application.Contracts.Interfaces.Services;

namespace NotificationService.Application.Platforms.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPlatforms(this IServiceCollection services)
        {
            services.AddTransient<IPlatformService, PlatformService>();
            return services;
        }
    }
}