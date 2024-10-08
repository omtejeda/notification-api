using Microsoft.AspNetCore.Http;
using NotificationService.Domain.Models;

namespace NotificationService.Application.Utils;

internal static class NotificationUtil
{
    public static IEnumerable<Attachment> GetCollection(this List<IFormFile>? attachments)
    {
        if (attachments is null) yield break;

        foreach (var attachment in attachments)
        {
            yield return new Attachment
            {
                FileName = GetUniqueFileName(attachment.FileName),
                OriginalFileName = attachment.FileName,
                ContentType = attachment.ContentType,
                Length = attachment.Length,
                FormFile = attachment
            };
        }
    }

    private static string GetUniqueFileName(string fileName)
    {
        return string.Concat(
            Path.GetFileNameWithoutExtension(fileName), 
            "_",
            Guid.NewGuid().ToString(),
            Path.GetExtension(fileName));
    }
}