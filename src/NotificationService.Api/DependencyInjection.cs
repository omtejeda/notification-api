using Microsoft.Extensions.DependencyInjection;
using NotificationService.Api.Extensions;
using NotificationService.Api.Utils;

namespace NotificationService.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApi(this IServiceCollection services)
        {
            services.AddAppCors();
            services.AddHttpClient();
            services.AddRouting(o => o.LowercaseUrls = true);
            services.AddControllers(o => o.UseGeneralRoutePrefix(Routes.GlobalPrefix));
            services.AddVersioning();
            services.AddAppSwagger();
            
            return services;
        }
    }
}