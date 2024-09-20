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

    private static string FormatPlaceholder(string key)
        => $"$[{key}]";
}