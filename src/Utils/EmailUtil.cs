using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using MimeKit;
using SendGrid.Helpers.Mail;

namespace NotificationService.Utils
{
    public static class EmailUtil
    {
        public static string ReplaceParameters(string text, IEnumerable<NotificationService.Dtos.Requests.MetadataDto> metadata)
        {
            var finalText = text;
            
            foreach(var meta in metadata)
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

        public static BodyBuilder AddAttachments(this BodyBuilder builder, System.Collections.Generic.List<Microsoft.AspNetCore.Http.IFormFile> attachments)
        {
            if (attachments != null)
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

        public static SendGridMessage AddAttachments(this SendGridMessage message, System.Collections.Generic.List<Microsoft.AspNetCore.Http.IFormFile> attachments)
        {
            if (attachments != null)
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

        public static void CheckSMTPSettings(string fromEmail, string host, int? port, string password)
        {
            if (string.IsNullOrWhiteSpace(fromEmail)) throw new ArgumentNullException(nameof(fromEmail));
            if (string.IsNullOrWhiteSpace(host)) throw new ArgumentNullException(nameof(host));
            if (!port.HasValue) throw new ArgumentNullException(nameof(port));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));
        }

        public static void CheckSendGridSettings(string fromEmail, string apiKey)
        {
            if (string.IsNullOrWhiteSpace(fromEmail)) throw new ArgumentNullException(nameof(fromEmail));
            if (string.IsNullOrWhiteSpace(apiKey)) throw new ArgumentNullException(nameof(apiKey));
        }
    }
}