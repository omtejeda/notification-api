using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Application.Catalogs.Services;

namespace NotificationService.Application.Catalogs.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCatalogs(this IServiceCollection services)
        {
            services.AddTransient<ICatalogService, CatalogService>();
            return services;
        }
    }
}