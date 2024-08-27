using System.Threading.Tasks;

namespace NotificationService.Contracts.Interfaces.Services;
public interface IWebhooksService
{
    Task<bool> SaveEmailContent(string content, string subject, string headers);
}
