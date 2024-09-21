using NotificationService.Domain.Enums;
using NotificationService.Domain.Models;
using NotificationService.SharedKernel;

namespace NotificationService.Domain.Entities;

public class Notification : EntityBase
{
    public string NotificationId { get; private set; } = string.Empty;
    public NotificationType Type { get; private set; }
    public string ToDestination { get; private set; } = string.Empty;
    public string TemplateName { get; private set; } = string.Empty;
    public string PlatformName { get; private set; } = string.Empty;
    public string ProviderName { get; private set; } = string.Empty;
    public DateTime? Date { get; private set; }
    public bool? Success { get; private set; }
    public string Message { get; private set; } = string.Empty;
    public bool HasAttachments { get; private set; } 
    public ICollection<Attachment> Attachments { get; private set; } = [];
    public object? Request { get; private set; }
    public string? ParentNotificationId { get; private set; }
    public string Content { get; private set; } = string.Empty;
    public string Subject { get; private set; } = string.Empty;
    public string From { get; private set; } = string.Empty;
    public int? TriesCount { get; private set; } = 0;
    private bool _savesAttachments;

    public bool MustSaveAttachments => _savesAttachments && HasAttachments;

    public void AddNotificationResult(NotificationResult notificationResult)
    {
        From = notificationResult.From;
        Success = notificationResult.IsSuccess;
        Message = notificationResult.Message;
        TriesCount = notificationResult.TriesCount;
        _savesAttachments = notificationResult.SavesAttachments;
    }

    public void Update(string subject, string content)
    {
        Subject = subject;
        Content = content;
    }

    public class Builder
    {
        private Notification _notification = new Notification();
        
        public static Builder NewNotification()
            => new Builder();
        
        public Builder OfType(NotificationType notificationType)
        {
            _notification.Type = notificationType;
            return this;
        }

        public Builder WithRuntimeTemplate(RuntimeTemplate runtimeTemplate)
        {
            _notification.TemplateName = runtimeTemplate.Name;
            _notification.PlatformName = runtimeTemplate.PlatformName;
            _notification.Content = runtimeTemplate.Content;
            _notification.Subject = runtimeTemplate.Subject;
            return this;
        }

        public Builder WithUserRequest(object request)
        {
            _notification.Request = request;
            return this;
        }

        public Builder From(string? from)
        {
            _notification.From = from ?? string.Empty;
            return this;
        }

        public Builder To(string to)
        {
            _notification.ToDestination = to;
            return this;
        }

        public Builder WithProviderName(string providerName)
        {
            _notification.ProviderName = providerName;
            return this;
        }

        public Builder WithAttachments(ICollection<Attachment> attachments)
        {
            _notification.Attachments = attachments;
            return this;
        }

        public Builder CreatedBy(string createdBy)
        {
            _notification.CreatedBy = createdBy;
            return this;
        }

        public Builder WasSuccess(bool? isSuccess)
        {
            _notification.Success = isSuccess;
            return this;
        }

        public Builder WithResultMessage(string resultMessage)
        {
            _notification.Message = resultMessage;
            return this;
        }

        public Builder WithTriesCount(int triesCount)
        {
            _notification.TriesCount = triesCount;
            return this;
        }

        public Builder HasParentNotificationId(string? parentNotificationId)
        {
            if (!string.IsNullOrWhiteSpace(parentNotificationId))
                _notification.ParentNotificationId = parentNotificationId;
            
            return this;
        }

        public Builder WithDate(DateTime date)
        {
            _notification.Date = date;
            return this;
        }

        public Notification Build()
        {
            _notification.NotificationId = Guid.NewGuid().ToString();
            _notification.HasAttachments = _notification.Attachments?.Any() ?? false;

            return _notification;
        }
    }
}