using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Application.Templates.Services;

namespace NotificationService.Application.Templates.IoC
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