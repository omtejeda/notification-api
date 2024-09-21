namespace NotificationService.Application.Contracts.Interfaces.Services;
public interface IWebhooksService
{
    Task<(bool, string)> SaveEmailContent(string content, string subject, string headers);
}
