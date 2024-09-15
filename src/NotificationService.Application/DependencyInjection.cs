using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Features.Catalogs;
using NotificationService.Application.Features.Notifications;
using NotificationService.Application.Features.Providers;
using NotificationService.Application.Platforms;
using NotificationService.Application.Features.Senders;
using NotificationService.Application.Templates;
using NotificationService.Application.Features.Webhooks;
using System.Reflection;

namespace NotificationService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
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