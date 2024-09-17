using NotificationService.Domain.Dtos;
using NotificationService.Domain.Models;

namespace NotificationService.Application.Features.Senders.Models
{
    public partial class EmailMessage
    {
        public class Builder
        {
            private EmailMessage _emailContent = new();

            public static Builder NewMessage()
                => new();

            public Builder To(string to)
            {
                _emailContent.To = to;
                return this;
            }

            public Builder WithCc(IEnumerable<string> cc)
            {
                _emailContent.Cc.AddRange(cc);
                return this;
            }

            public Builder WithBcc(IEnumerable<string> bcc)
            {
                _emailContent.Bcc.AddRange(bcc);
                return this;
            }

            public Builder WithSubject(string subject)
            {
                _emailContent.Subject = subject;
                return this;
            }

            public Builder WithContent(string content)
            {
                _emailContent.Content = content;
                return this;
            }

            public Builder WithAttachments(IEnumerable<Attachment> attachments)
            {
                _emailContent.Attachments.AddRange(attachments);
                return this;
            }

            public Builder AddHeader (string key, string value)
            {
                if(key is not null && value is not null)
                    _emailContent.Headers[key] = value;

                return this;
            }
            public Builder WithHeaders(Dictionary<string, string> headers)
            {
                foreach(var entry in headers)
                    _emailContent.Headers[entry.Key] = entry.Value;

                return this;
            }

            
            public Builder UsingMetadata(IEnumerable<MetadataDto> providedMetadata)
            {
                _emailContent.ProvidedMetadata.AddRange(providedMetadata);
                return this;
            }

            public EmailMessage Build()
            {
                return _emailContent;
            }
        }
    }
}