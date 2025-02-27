using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Common.Services;
using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Common;

public static class DependencyInjection
{
    public static IServiceCollection AddCommon(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeService, DateTimeService>();
        services.AddSingleton<IEnvironmentService, EnvironmentService>();
        return services;
    }
}