using NotificationService.Domain.Entities;

namespace NotificationService.Domain.Models;

public class FirebaseNotification
{
    public string[] UserTokens { get; set; } = [];
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public FirebaseSetting FirebaseSetting { get; set; } = new();
}