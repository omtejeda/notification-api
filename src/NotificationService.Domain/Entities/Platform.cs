using NotificationService.SharedKernel;

namespace NotificationService.Domain.Entities;

public class Platform : EntityBase
{
    public string PlatformId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = false;
    public bool IsAdmin { get; set; } = false;
    public string ApiKey { get; set; } = string.Empty;
}