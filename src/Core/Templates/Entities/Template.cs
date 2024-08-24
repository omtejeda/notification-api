using System.Collections.Generic;
using NotificationService.Core.Common.Entities;
using NotificationService.Core.Common.Enums;

namespace NotificationService.Core.Templates.Entities
{
    public class Template : BaseEntity
    {
        public string TemplateId { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public Language Language { get; set; }
        public NotificationType NotificationType { get; set; }
        public string PlatformName { get; set; }
        public string Content { get; set; }
        public string Location { get; set; }
        public ICollection<Metadata> Metadata { get; set; }
        public ICollection<TemplateLabel> Labels { get; set; }
    }

    public class TemplateLabel
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string CatalogNameToCheckAgainst { get; set; }
    }
}