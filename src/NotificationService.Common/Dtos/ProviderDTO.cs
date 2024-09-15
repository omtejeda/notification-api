namespace NotificationService.Common.Dtos;

public class ProviderDto
{
    public string ProviderId { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public ProviderSettingsDto Settings { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsPublic { get; set; }
    public ProviderDevSettingsDto DevSettings { get; set; }
    public bool SavesAttachments { get; set; } = false;
}

public class ProviderDevSettingsDto
{
    public ICollection<string> AllowedRecipients { get; set; } = new List<string>();
}

public class ProviderSettingsDto
{
    public SendGridSettingDto SendGrid { get; set; }
    public SMTPSettingDto Smtp { get; set; }
    public HttpClientSettingDto HttpClient { get; set; }
}

public class HttpClientSettingDto
{
    public string Host { get; set; }
    public string Uri { get; set; }
    public string Verb { get; set; }
    public ICollection<HttpClientParamDto> Params { get; set; } = new List<HttpClientParamDto>();
    public JsonBodyDto JsonBody { get; set; }
}

public class HttpClientParamDto
{
    public string Type { get; set; }
    public string Name { get; set; }
    public bool? IsRequired { get; set; }
    public bool? HasStaticValue { get; set; }
    public string StaticValue { get; set; }
    public string ReadValueFrom { get; set; }
}

public class SendGridSettingDto
{
    public string FromEmail { get; set; }
    public string FromDisplayName { get; set; }
    public string ApiKey { get; set; }
}

public class SMTPSettingDto
{
    public string FromEmail { get; set; }
    public string FromDisplayName { get; set; }
    public string Host { get; set; }
    public int? Port { get; set; }
    public string Password { get; set; }
    public bool? Authenticate { get; set; }
}

public class JsonBodyDto
{
    public string RootIs { get; set; } = string.Empty;
    public List<JsonKeyDto> Definition { get; set; } = new();
}

public class JsonKeyDto
{
    public string PropertyName { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
    public bool IsRequired { get; set; }
    public List<JsonKeyDto> Childs { get; set; } = new();
}