using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Features.Senders;
using NotificationService.Application.Interfaces;

namespace NotificationService.Application.Features.Senders
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