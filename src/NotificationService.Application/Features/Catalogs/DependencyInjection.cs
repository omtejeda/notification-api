using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Contracts.Services;
using NotificationService.Application.Features.Catalogs.Services;

namespace NotificationService.Application.Features.Catalogs;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogs(this IServiceCollection services)
    {
        services.AddTransient<ICatalogService, CatalogService>();
        return services;
    }
}