using Microsoft.Extensions.DependencyInjection;
using NotificationService.Contracts.Interfaces.Services;
using NotificationService.Core.Webhooks.Services;

namespace NotificationService.Core.Webhooks.IoC
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