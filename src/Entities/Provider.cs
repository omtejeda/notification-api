using System;
using System.Collections.Generic;
using NotificationService.Enums;
namespace NotificationService.Entities
{
    public class Provider : BaseEntity
    {
        public string ProviderId { get; set; }
        public string Name { get; set; }
        public ProviderType Type { get; set; }
        public ProviderSettings Settings { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsPublic { get; set; }
        public ProviderDevSettings DevSettings { get; set; }
    }

    public class ProviderDevSettings
    {
        public ICollection<string> AllowedRecipients { get; set; }
    }

    public class ProviderSettings
    {
        public SendGridSetting SendGrid { get; set; }
        public SMTPSetting Smtp { get; set; }
        public HttpClientSetting HttpClient { get; set; }
    }

    public class HttpClientSetting
    {
        public string Host { get; set; }
        public string Uri { get; set; }
        public string Verb { get; set; }
        public ICollection<HttpClientParam> Params { get; set; }
    }

    public class HttpClientParam
    {
        public HttpClientParamType Type { get; set; }
        public string Name { get; set; }
        public bool? IsRequired { get; set; }
        public bool? HasStaticValue { get; set; }
        public string StaticValue { get; set; }
        public HttpClientParamValueReader ReadValueFrom { get; set; }
    }

    public enum HttpClientParamType
    {
        None,
        QueryString,
        Body,
        Header,
        Route
    }

    public enum HttpClientParamValueReader
    {
        None,
        TemplateContent,
        RequestMetadata,
        RequestToDestination
    }

    public class SendGridSetting
    {
        public string FromEmail { get; set; }
        public string FromDisplayName { get; set; }
        public string ApiKey { get; set; }
    }

    public class SMTPSetting
    {
        public string FromEmail { get; set; }
        public string FromDisplayName { get; set; }
        public string Host { get; set; }
        public int? Port { get; set; }
        public string Password { get; set; }
        public bool? Authenticate { get; set; }
    }
}