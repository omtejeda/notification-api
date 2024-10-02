using System.Text.Json;
using System.Text.Json.Serialization;

namespace NotificationService.Api.Utils;

public static class JsonUtils
{
    public static JsonSerializerOptions GetDefaultJsonSerializerOptions()
    {
        return new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }
}