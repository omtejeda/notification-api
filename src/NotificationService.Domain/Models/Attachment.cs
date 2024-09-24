using Microsoft.AspNetCore.Http;

namespace NotificationService.Domain.Models;

public class Attachment
{
    public string FileName { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long Length { get; set; }
    public IFormFile? FormFile { get; set; }
}