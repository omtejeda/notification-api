using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Interfaces;

namespace NotificationService.Application.Senders.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSenders(this IServiceCollection services)
        {
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ISmsSender, SmsSender>();
            services.AddTransient<IMessageSender, MessageSender>();
            return services;
        }
    }
}