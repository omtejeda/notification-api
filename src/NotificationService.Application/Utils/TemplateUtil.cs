using System.Text;
using NotificationService.Domain.Dtos;

namespace NotificationService.Application.Utils;

public static class TemplateUtil
{
    public static string ReplaceParameters(string text, IEnumerable<MetadataDto> metadata)
    {        
        var content = new StringBuilder(text);
        foreach(var placeholder in metadata)
        {
            content.Replace(FormatPlaceholder(placeholder.Key), placeholder.Value);
        }

        return content.ToString();
    }

    public static bool ContainsAllPlaceholders(string text, IEnumerable<MetadataDto> metadata)
    {
        foreach(var placeholder in metadata)
        {
            if (!text.Contains(FormatPlaceholder(placeholder.Key)))
                return false;
        }
        return true;
    }

    private static string FormatPlaceholder(string key)
        => $"$[{key}]";
}