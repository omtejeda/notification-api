using NotificationService.Domain.Enums;
namespace NotificationService.Domain.Entities;

public class Provider : BaseEntity
{
    public string ProviderId { get; set; }
    public string Name { get; set; }
    public ProviderType Type { get; set; }
    public ProviderSettings Settings { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsPublic { get; set; }
    public ProviderDevSettings DevSettings { get; set; }
    public bool SavesAttachments { get; set; } = false;
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
    public JsonBody JsonBody { get; set; }
}

public class HttpClientParam
{
    public HttpClientParamType Type { get; set; }
    public string Name { get; set; }
    public bool? IsRequired { get; set; }
    public bool? HasStaticValue { get; set; }
    public string StaticValue { get; set; }
    public HttpClientParamValueReader ReadValueFrom { get; set; }

    public bool IsValid(string value)
        => (IsRequired == true && !string.IsNullOrEmpty(value)) || IsRequired == false;
}

public enum HttpClientParamType
{
    None,
    QueryString,
    JsonBodyDefinition,
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

public class JsonBody
{
    public string RootIs { get; set; }
    public List<JsonKey> Definition { get; set; } = new();
}

public class JsonKey
{
    public string PropertyName { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
    public bool IsRequired { get; set; }
    public List<JsonKey> Childs { get; set; } = new();
}