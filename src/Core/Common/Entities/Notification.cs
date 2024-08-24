using System;
using System.Collections.Generic;
using System.Linq;
using NotificationService.Core.Common.Enums;

namespace NotificationService.Core.Common.Entities
{
    public class Notification : BaseEntity
    {
        public string NotificationId { get; private set; }
        public NotificationType Type { get; private set; }
        public string ToDestination { get; private set; }
        public string TemplateName { get; private set; }
        public string PlatformName { get; private set; }
        public string ProviderName { get; private set; }
        public DateTime? Date { get; private set; }
        public bool? Success { get; private set; }
        public string Message { get; private set; }
        public bool HasAttachments { get; private set; } 
        public ICollection<Attachment> Attachments { get; private set; }
        public object Request { get; private set; }
        public string ParentNotificationId { get; private set; }
        public string Content { get; private set; }
        public string Subject { get; private set; }
        public string From { get; private set; }
        public int? TriesCount { get; private set; } = 0;
        private bool _savesAttachments;

        public bool AppliesSavesAttachments => _savesAttachments && HasAttachments;

        internal void AddNotificationResult(NotificationResult notificationResult)
        {
            From = notificationResult.From;
            Success = notificationResult.IsSuccess;
            Message = notificationResult.Message;
            TriesCount = notificationResult.TriesCount;
            _savesAttachments = notificationResult.SavesAttachments;
        }

        internal void Update(string subject, string content)
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

            public Builder WithRuntimeTemplate(Core.Templates.Models.RuntimeTemplate runtimeTemplate)
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

            public Builder From(string from)
            {
                _notification.From = from;
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

            public Builder HasParentNotificationId(string parentNotificationId)
            {
                _notification.ParentNotificationId = parentNotificationId;
                return this;
            }

            public Notification Build()
            {
                _notification.NotificationId = Guid.NewGuid().ToString();
                _notification.Date = Core.Common.Utils.SystemUtil.GetSystemDate();
                _notification.HasAttachments = _notification.Attachments?.Any() ?? false;

                return _notification;
            }
        }
    }
}