using NotificationService.Domain.Enums;
using NotificationService.SharedKernel;

namespace NotificationService.Domain.Entities;

public class Provider : EntityBase
{
    public string ProviderId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public ProviderType Type { get; set; }
    public ProviderSettings Settings { get; set; } = new();
    public bool? IsActive { get; set; }
    public bool? IsPublic { get; set; }
    public ProviderDevSettings DevSettings { get; set; } = new();
    public bool SavesAttachments { get; set; } = false;
}

public class ProviderDevSettings
{
    public ICollection<string> AllowedRecipients { get; set; } = [];
}

public class ProviderSettings
{
    public SendGridSetting? SendGrid { get; set; }
    public SMTPSetting? Smtp { get; set; }
    public HttpClientSetting? HttpClient { get; set; }
}

public class HttpClientSetting
{
    public string Host { get; set; } = string.Empty;
    public string Uri { get; set; } = string.Empty;
    public string Verb { get; set; } = string.Empty;
    public ICollection<HttpClientParam> Params { get; set; } = [];
    public JsonBody? JsonBody { get; set; }
}

public class HttpClientParam
{
    public HttpClientParamType Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool? IsRequired { get; set; }
    public bool? HasStaticValue { get; set; }
    public string StaticValue { get; set; } = string.Empty;
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
    public string FromEmail { get; set; } = string.Empty;
    public string FromDisplayName { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
}

public class SMTPSetting
{
    public string FromEmail { get; set; } = string.Empty;
    public string FromDisplayName { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Password { get; set; } = string.Empty;
    public bool? Authenticate { get; set; }
}

public class JsonBody
{
    public string RootIs { get; set; } = string.Empty;
    public List<JsonKey> Definition { get; set; } = new();
}

public class JsonKey
{
    public string PropertyName { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
    public bool IsRequired { get; set; }
    public List<JsonKey> Childs { get; set; } = new();
}