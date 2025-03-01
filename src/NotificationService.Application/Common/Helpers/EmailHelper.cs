using MimeKit;
using NotificationService.Domain.Entities;
using SendGrid.Helpers.Mail;
using NotificationService.Domain.Dtos;
using NotificationService.Application.Features.Templates.Models;
using NotificationService.Domain.ValueObjects;

namespace NotificationService.Application.Common.Helpers;

public static class EmailHelper
{
    public static BodyBuilder AddAttachments(this BodyBuilder builder, List<Microsoft.AspNetCore.Http.IFormFile>? attachments)
    {
        if (attachments is null) return builder;

        byte[] fileBytes;
        foreach (var file in attachments)
        {
            if (file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
                builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
            }
        }
        return builder;
    }

    public static SendGridMessage AddAttachments(this SendGridMessage message, List<Microsoft.AspNetCore.Http.IFormFile>? attachments)
    {
        if (attachments is null) return message;
        
        string fileBase64;
        foreach (var file in attachments)
        {
            if (file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    fileBase64 = Convert.ToBase64String(ms.ToArray());
                }
                message.AddAttachment(file.FileName, fileBase64);
            }
        }
        return message;
    }        

    public static SendGridTemplate GetSendgridTemplateFromMetadata(List<MetadataDto> providedMetadata)
    {
        var result = new SendGridTemplate();
        if (providedMetadata != null)
        {
            result.TemplateId = providedMetadata.FirstOrDefault(x => x.Key.ToLowerInvariant() == Parameters.SendgridTemplateId)?.Value;
            result.Category = providedMetadata.FirstOrDefault(x => x.Key.ToLowerInvariant() == Parameters.SendgridCategory)?.Value;
            result.HasTemplate = result.TemplateId != null;
            if (result.HasTemplate)
                result.DynamicTemplateData = providedMetadata.Where(x => !Parameters.ParameterList.Contains(x.Key)).ToDictionary(k => k.Key, v => v.Value);
        }

        return result;
    }

    public static void ThrowIfEmailNotAllowed(string? environment, Provider provider, Email? to, IEnumerable<Email>? cc = null, IEnumerable<Email>? bcc = null)
    {   
        Guard.CanSendToDestination(provider, to ?? string.Empty, environment);

        foreach (var ccEmail in cc ?? [])
            Guard.CanSendToDestination(provider, ccEmail!, environment);

        foreach (var bccEmail in bcc ?? [])
            Guard.CanSendToDestination(provider, bccEmail!, environment);
    }

    internal static class Parameters
    {
        public static readonly string SendgridTemplateId = "x-template-id";
        public static readonly string SendgridCategory = "x-category";
        public static readonly string NotificationIdHeader = "x-notification-id";

        public static readonly List<string> ParameterList = new List<string> { SendgridTemplateId, SendgridCategory, NotificationIdHeader };

    }
}