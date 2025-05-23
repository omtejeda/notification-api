using NotificationService.Application.Contracts.Services;
using NotificationService.Application.Contracts.DTOs.Responses;
using NotificationService.Domain.Enums;

namespace NotificationService.Application.Features.Notifications.Services;

public class EmlExportNotificationService : IExportNotificationsService
{
    public ExportFormat ExportFormat => ExportFormat.Eml;

    private const string MIME_CONTENT_TYPE = "multipart/mixed";
    private const string MIME_BOUNDARY = "delimiter";
    private const string MIME_BODY_CONTENT_TYPE = "text/html";
    private  const string MIME_BODY_CHARSET = "UTF-8";
    private const string MIME_ATTACHMENTS_CONTENT_DISPOSITION = "attachment";
    private const string MIME_ATTACHMENTS_CONTENT_ENCODING = "base64";
    private static readonly string SECTION_BOUNDARY = $"\r\n\r\n--{MIME_BOUNDARY}";
    private static readonly string FINAL_BOUNDARY = $"{SECTION_BOUNDARY}--";
    private readonly INotificationsService _notificationsService;

    public EmlExportNotificationService(INotificationsService notificationsService)
    {
        _notificationsService = notificationsService;
    }

    public async Task<ExportNotificationResponseDto> Export(string notificationId, string owner)
    {
        var notificationDetail = await _notificationsService.GetNotificationById(notificationId, owner);
        if (notificationDetail is null || notificationDetail.Data is null)
            return default!;
            
        var notification = notificationDetail.Data ?? throw new ArgumentNullException(nameof(notificationDetail.Data));
        var emlContent = @$"From: {notification.From}
        To: {notification.ToDestination}
        Subject: {notification.Subject}
        Date: {notification.Date}
        Content-Type: {MIME_CONTENT_TYPE}; boundary=""{MIME_BOUNDARY}""
        
        --{MIME_BOUNDARY}
        Content-Type: {MIME_BODY_CONTENT_TYPE}; charset=""{MIME_BODY_CHARSET}""

        {notification.Content}
        ";

        var result = new ExportNotificationResponseDto
        {
            Content = emlContent,
            ContentType = "text/plain"
        };

        result.Content = RemoveLeadingSpaces(result.Content);

        if (notification.HasAttachments)
        {
            await ExportAttachmentsAsync(result, notification.Attachments!);
        }
        result.Content += FINAL_BOUNDARY;

        return result;
    }

    private static string RemoveLeadingSpaces(string value)
        => string.Join("\n", value.Split('\n').Select(line => line.TrimStart()));

    private async Task ExportAttachmentsAsync(ExportNotificationResponseDto exportNotification, ICollection<AttachmentDto> attachments)
    {
        var attachmentsContent = _notificationsService.GetAttachmentsAsBase64(attachments);
        await foreach (var attachment in attachmentsContent)
        {
            var encodedContent = @$"Content-Type: {attachment.ContentType}
            Content-Disposition: {MIME_ATTACHMENTS_CONTENT_DISPOSITION}; filename=""{attachment.FileName}""
            Content-Transfer-Encoding: {MIME_ATTACHMENTS_CONTENT_ENCODING}

            {attachment.EncodedContent}
            ";
            encodedContent = RemoveLeadingSpaces(encodedContent);

            exportNotification.Content += $"{SECTION_BOUNDARY}\r\n{encodedContent}";
        }
    }
}