using System;

namespace NotificationService.Common.Entities
{
    public class Metadata
    {
        public string Key { get; set; }
        public string Description { get; set; }
        public bool IsRequired { get; set; }
    }
}