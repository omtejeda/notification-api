using Microsoft.Extensions.DependencyInjection;
using NotificationService.Core.Contracts.Interfaces.Services;
using NotificationService.Core.Templates.Services;

namespace NotificationService.Core.Templates.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddTemplates(this IServiceCollection services)
        {
            services.AddTransient<ITemplateService, TemplateService>();
            return services;
        }
    }
}