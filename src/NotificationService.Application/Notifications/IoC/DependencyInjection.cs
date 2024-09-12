using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Application.Notifications.Services;
using NotificationService.Application.Notifications.Factories;
using NotificationService.Application.Contracts.Interfaces.Factories;

namespace NotificationService.Application.Notifications.IoC
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