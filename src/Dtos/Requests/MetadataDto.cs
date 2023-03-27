using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using NotificationService.Attributes;
using NotificationService.Enums;
using NotificationService.Entities;
using System.Text.Json.Serialization;

namespace NotificationService.Dtos.Requests
{
    public class MetadataDto
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}