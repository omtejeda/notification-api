using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Application.Contracts.Interfaces.Factories;
using NotificationService.Application.Features.Notifications.Services;
using NotificationService.Application.Features.Notifications.Factories;

namespace NotificationService.Application.Features.Notifications
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