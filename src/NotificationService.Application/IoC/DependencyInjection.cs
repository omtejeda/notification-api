using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Catalogs.IoC;
using NotificationService.Application.Notifications.IoC;
using NotificationService.Application.Platforms.IoC;
using NotificationService.Application.Providers.IoC;
using NotificationService.Application.Senders.IoC;
using NotificationService.Application.Templates.IoC;
using NotificationService.Application.Webhooks.IoC;
using System.Reflection;

namespace NotificationService.Application.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
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