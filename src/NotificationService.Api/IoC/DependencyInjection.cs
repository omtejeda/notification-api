using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Api.Extensions;
using NotificationService.Api.Utils;

namespace NotificationService.Api.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApi(this IServiceCollection services)
        {
            services.AddAppCors();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddHttpClient();

            services.AddRouting(o => o.LowercaseUrls = true);
            services.AddControllers(o => o.UseGeneralRoutePrefix(Routes.GlobalPrefix));

            services.AddVersioning();
            services.AddAppSwagger();
            
            return services;
        }
    }
}