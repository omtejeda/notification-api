namespace NotificationService.Common.Dtos;

public class PlatformDto
{
    public string PlatformId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool? IsActive { get; set; }
    public bool? IsAdmin { get; set; }
    public string ApiKey { get; set; } = string.Empty;
}