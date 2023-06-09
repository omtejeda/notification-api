using System;
using System.Collections.Generic;

namespace NotificationService.Entities
{
    public class Platform : BaseEntity
    {
        public string PlatformId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsAdmin { get; set; }
        public string ApiKey { get; set; }
    }
}