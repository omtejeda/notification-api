using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Application.Webhooks.Services;

namespace NotificationService.Application.Webhooks.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebhooks(this IServiceCollection services)
        {
            services.AddTransient<IWebhooksService, WebhooksService>();
            return services;
        }
    }
}