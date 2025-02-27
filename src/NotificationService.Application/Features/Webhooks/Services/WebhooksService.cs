using NotificationService.Application.Contracts.Persistence;
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Domain.Entities;
using static NotificationService.Application.Utils.EmailUtil;

namespace NotificationService.Application.Features.Webhooks.Services;

public class WebhooksService(IRepository<Notification> notificationRepository) : IWebhooksService
{
    private readonly IRepository<Notification> _notificationRepository = notificationRepository;

    /// <summary>
    /// Look up successful notifications related to the email that was previously sent (by correlation or email attributes)
    /// Take the most recent notification that meets the criteria
    /// Update the notification content using content supplied by SendGrid
    /// </summary>
    /// <param name="content"></param>
    /// <param name="subject"></param>
    /// <param name="headers"></param>
    /// <returns>A boolean task indicating whether was successful</returns>
    public async Task<(bool, string)> SaveEmailContent(string content, string subject, string headers)
    {
        if (content is null || subject is null || headers is null)
            return (false, string.Empty);

        var headersDict = ParseHeaders(headers);
        if (headersDict is null)
            return (false, string.Empty);

        var notificationId = headersDict[Parameters.NotificationIdHeader];
        if (notificationId is null)
            return (false, string.Empty);

        var notification = await _notificationRepository.FindOneAsync(x => x.NotificationId == notificationId);

        if (notification is null)
            return (false, string.Empty);

        var documentId = notification.Id;

        notification.Update(subject, content);

        var result = await _notificationRepository.UpdateOneByIdAsync(documentId, notification);
        return (result, notificationId);
    }

    public static Dictionary<string, string> ParseHeaders(string headersData)
    {
        var result = new Dictionary<string, string>();
        var lines = headersData.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            var parts = line.Split(':');
            if (parts.Length == 2)
            {
                var key = parts[0].Trim();
                var value = parts[1].Trim();
                result[key] = value;
            }
        }

        return result;
    }
}