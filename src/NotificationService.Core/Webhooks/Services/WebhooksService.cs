using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NotificationService.Core.Contracts.Interfaces.Repositories;
using NotificationService.Core.Contracts.Interfaces.Services;
using NotificationService.Domain.Entities;
using static NotificationService.Core.Common.Utils.EmailUtil;

namespace NotificationService.Core.Webhooks.Services;

public class WebhooksService : IWebhooksService
{
    private readonly IRepository<Notification> _notificationRepository;

    public WebhooksService(IRepository<Notification> notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    /// <summary>
    /// Look up successful notifications related to the email that was previously sent (by correlation or email attributes)
    /// Take the most recent notification that meets the criteria
    /// Update the notification content using content supplied by SendGrid
    /// </summary>
    /// <param name="content"></param>
    /// <param name="subject"></param>
    /// <param name="headers"></param>
    /// <returns>A boolean task indicating whether was successful</returns>
    public async Task<bool> SaveEmailContent(string content, string subject, string headers)
    {
        if (content is null || subject is null || headers is null)
            return false;

        var headersDict = ParseHeaders(headers);
        if (headersDict is null)
            return false;

        var notificationId = headersDict[Parameters.NotificationIdHeader];
        if (notificationId is null)
            return false;

        var notification = await _notificationRepository.FindOneAsync(x => x.NotificationId == notificationId);

        if (notification is null)
            return false;

        var documentId = notification.Id;

        notification.Update(subject, content);

        var result = await _notificationRepository.UpdateOneByIdAsync(documentId, notification);
        return result;
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