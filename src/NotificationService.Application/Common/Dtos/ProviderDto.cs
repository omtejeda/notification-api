namespace NotificationService.Application.Common.Dtos;

public class ProviderDto
{
    public string ProviderId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public ProviderSettingsDto Settings { get; set; } = new();
    public bool? IsActive { get; set; }
    public bool? IsPublic { get; set; }
    public ProviderDevSettingsDto DevSettings { get; set; } = new();
    public bool SavesAttachments { get; set; } = false;
}

public class ProviderDevSettingsDto
{
    public ICollection<string> AllowedRecipients { get; set; } = new List<string>();
}

public class ProviderSettingsDto
{
    public SendGridSettingDto? SendGrid { get; set; }
    public SmtpSettingDto? Smtp { get; set; }
    public HttpClientSettingDto? HttpClient { get; set; }
    public FirebaseSettingDto? Firebase { get; set; }
}

public class HttpClientSettingDto
{
    public string Host { get; set; } = string.Empty;
    public string Uri { get; set; } = string.Empty;
    public string Verb { get; set; } = string.Empty;
    public ICollection<HttpClientParamDto> Params { get; set; } = [];
    public JsonBodyDto? JsonBody { get; set; }
}

public class HttpClientParamDto
{
    public string Type { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool? IsRequired { get; set; }
    public bool? HasStaticValue { get; set; }
    public string StaticValue { get; set; } = string.Empty;
    public string ReadValueFrom { get; set; } = string.Empty;
}

public class SendGridSettingDto
{
    public string FromEmail { get; set; } = string.Empty;
    public string FromDisplayName { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
}

public class SmtpSettingDto
{
    public string FromEmail { get; set; } = string.Empty;
    public string FromDisplayName { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public int? Port { get; set; }
    public string Password { get; set; } = string.Empty;
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

public class FirebaseSettingDto
{
    public string Type { get; set; } = string.Empty;
    public string ProjectId { get; set; } = string.Empty;
    public string PrivateKeyId { get; set; } = string.Empty;
    public string PrivateKey { get; set; } = string.Empty;
    public string ClientEmail { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string AuthUri { get; set; } = string.Empty;
    public string TokenUri { get; set; } = string.Empty;
    public string AuthProviderX509CertUrl { get; set; } = string.Empty;
    public string ClientX509CertUrl { get; set; } = string.Empty;
    public string UniverseDomain { get; set; } = string.Empty;
}