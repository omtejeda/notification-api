using Microsoft.Extensions.DependencyInjection;
using NotificationService.Core.Catalogs.IoC;
using NotificationService.Core.Notifications.IoC;
using NotificationService.Core.Platforms.IoC;
using NotificationService.Core.Providers.IoC;
using NotificationService.Core.Senders.IoC;
using NotificationService.Core.Templates.IoC;
using NotificationService.Core.Webhooks.IoC;

namespace NotificationService.Core.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddCatalogs();
            services.AddNotififications();
            services.AddPlatforms();
            services.AddProviders();
            services.AddSenders();
            services.AddTemplates();
            services.AddWebhooks();
            return services;
        }
    }
}