using Microsoft.Extensions.DependencyInjection;
using NotificationService.Core.Contracts.Interfaces.Services;
using NotificationService.Core.Notifications.Services;
using NotificationService.Core.Notifications.Factories;
using NotificationService.Core.Contracts.Interfaces.Factories;

namespace NotificationService.Core.Notifications.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddNotififications(this IServiceCollection services)
        {
            services.AddTransient<INotificationsService, NotificationsService>();
            services.AddTransient<IExportNotificationsService, EmlExportNotificationService>();
            services.AddTransient<IExportNotificationsFactory, ExportNotificationsFactory>();
            return services;
        }
    }
}