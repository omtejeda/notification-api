namespace NotificationService.Common.Dtos
{
    public class ProviderDTO
    {
        public string ProviderId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public ProviderSettingsDTO Settings { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsPublic { get; set; }
        public ProviderDevSettingsDTO DevSettings { get; set; }
        public bool SavesAttachments { get; set; } = false;
    }

    public class ProviderDevSettingsDTO
    {
        public ICollection<string> AllowedRecipients { get; set; } = new List<string>();
    }

    public class ProviderSettingsDTO
    {
        public SendGridSettingDTO SendGrid { get; set; }
        public SMTPSettingDTO Smtp { get; set; }
        public HttpClientSettingDTO HttpClient { get; set; }
    }

    public class HttpClientSettingDTO
    {
        public string Host { get; set; }
        public string Uri { get; set; }
        public string Verb { get; set; }
        public ICollection<HttpClientParamDTO> Params { get; set; } = new List<HttpClientParamDTO>();
        public JsonBodyDTO JsonBody { get; set; }
    }

    public class HttpClientParamDTO
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public bool? IsRequired { get; set; }
        public bool? HasStaticValue { get; set; }
        public string StaticValue { get; set; }
        public string ReadValueFrom { get; set; }
    }

    public class SendGridSettingDTO
    {
        public string FromEmail { get; set; }
        public string FromDisplayName { get; set; }
        public string ApiKey { get; set; }
    }

    public class SMTPSettingDTO
    {
        public string FromEmail { get; set; }
        public string FromDisplayName { get; set; }
        public string Host { get; set; }
        public int? Port { get; set; }
        public string Password { get; set; }
        public bool? Authenticate { get; set; }
    }

    public class JsonBodyDTO
    {
        public string RootIs { get; set; } = string.Empty;
        public List<JsonKeyDTO> Definition { get; set; } = new();
    }

    public class JsonKeyDTO
    {
        public string PropertyName { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public bool IsRequired { get; set; }
        public List<JsonKeyDTO> Childs { get; set; } = new();
    }
}