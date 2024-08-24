using System.Collections.Generic;
using NotificationService.Contracts.RequestDtos;
using NotificationService.Core.Common.Entities;

namespace NotificationService.Core.Templates.Dtos
{
    public class TemplateDTO
    {
        public string TemplateId { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Language { get; set; }
        public string NotificationType { get; set; }
        public string PlatformName { get; set; }
        public ICollection<Metadata> Metadata { get; set; }
        public ICollection<TemplateLabelDTO> Labels { get; set; }
    }
}