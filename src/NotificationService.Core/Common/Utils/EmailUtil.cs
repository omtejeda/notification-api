using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MimeKit;
using NotificationService.Common.Entities;
using NotificationService.Core.Common.Exceptions;
using SendGrid.Helpers.Mail;
using NotificationService.Common.Dtos;
using NotificationService.Core.Templates.Models;
using NotificationService.Common.Utils;

namespace NotificationService.Core.Common.Utils
{
    internal static class EmailUtil
    {
        public static string ReplaceParameters(string text, IEnumerable<MetadataDto> metadata)
        {
            var finalText = text;

            foreach (var meta in metadata)
            {
                finalText = finalText.Replace($"$[{meta.Key}]", meta.Value);
            }
            return finalText;
        }

        public static string ReadFile(string path)
        {
            StreamReader str = new StreamReader(path);
            var text = str.ReadToEnd();
            str.Close();

            return text;
        }

        public static BodyBuilder AddAttachments(this BodyBuilder builder, List<Microsoft.AspNetCore.Http.IFormFile> attachments)
        {
            if (attachments is not null)
            {
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
            }
            return builder;
        }

        public static SendGridMessage AddAttachments(this SendGridMessage message, List<Microsoft.AspNetCore.Http.IFormFile> attachments)
        {
            if (attachments is not null)
            {
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
            }
            return message;
        }        

        public static SendgridTemplate GetSendgridTemplateFromMetadata(List<MetadataDto> providedMetadata)
        {
            var result = new SendgridTemplate();
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

        public static void ThrowIfEmailNotAllowed(Provider provider, string to = null, ICollection<string> cc = null, ICollection<string> bcc = null)
        {
            ThrowIfEmailNotAllowed(provider, to);

            foreach (var ccEmail in cc ?? Enumerable.Empty<string>())
                ThrowIfEmailNotAllowed(provider, ccEmail);

            foreach (var bccEmail in bcc ?? Enumerable.Empty<string>())
                ThrowIfEmailNotAllowed(provider, bccEmail);
        }

        private static void ThrowIfEmailNotAllowed(Provider provider, string to)
        {
            if (SystemUtil.IsProduction()) return;

            var isEmailAllowed = provider?.DevSettings?.AllowedRecipients?.Any(x => x.ToLower() == to.ToLower()) ?? false;
            if (!isEmailAllowed)
            {
                throw new RuleValidationException($"Not allowed sending to {to} in non production environment");
            }
        }

        internal static class Parameters
        {
            public static readonly string SendgridTemplateId = "x-template-id";
            public static readonly string SendgridCategory = "x-category";
            public static readonly string NotificationIdHeader = "x-notification-id";

            public static readonly List<string> ParameterList = new List<string> { SendgridTemplateId, SendgridCategory, NotificationIdHeader };

        }
    }
}