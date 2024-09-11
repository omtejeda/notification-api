using System.Threading.Tasks;

namespace NotificationService.Core.Contracts.Interfaces.Services;
public interface IWebhooksService
{
    Task<bool> SaveEmailContent(string content, string subject, string headers);
}
