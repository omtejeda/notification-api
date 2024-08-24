using Microsoft.Extensions.DependencyInjection;
using NotificationService.Core.Platforms.Services;
using NotificationService.Contracts.Interfaces.Services;

namespace NotificationService.Core.Platforms.IoC
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