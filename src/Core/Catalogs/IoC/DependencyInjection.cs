using Microsoft.Extensions.DependencyInjection;
using NotificationService.Contracts.Interfaces.Services;
using NotificationService.Core.Catalogs.Services;

namespace NotificationService.Core.Catalogs.IoC
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