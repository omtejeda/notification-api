using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Features.Senders.Commands.SendEmail;
using NotificationService.Application.Features.Senders.Commands.SendMessage;
using NotificationService.Application.Features.Senders.Commands.SendSms;
using NotificationService.Application.Contracts.Senders;
using NotificationService.Application.Features.Senders.Commands.SendPush;

namespace NotificationService.Application.Features.Senders;

public static class DependencyInjection
{
    public static IServiceCollection AddSenders(this IServiceCollection services)
    {
        services.AddTransient<IEmailSender, EmailSender>();
        services.AddTransient<ISmsSender, SmsSender>();
        services.AddTransient<IMessageSender, MessageSender>();
        services.AddTransient<IPushSender, PushSender>();
        return services;
    }
}