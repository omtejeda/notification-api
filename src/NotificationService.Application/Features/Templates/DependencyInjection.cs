using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Contracts.Services;
using NotificationService.Application.Features.Templates.Services;

namespace NotificationService.Application.Templates;

public static class DependencyInjection
{
    public static IServiceCollection AddTemplates(this IServiceCollection services)
    {
        services.AddTransient<ITemplateService, TemplateService>();
        return services;
    }
}