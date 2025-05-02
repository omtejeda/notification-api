using NotificationService.Domain.Models;

namespace NotificationService.Application.Features.Providers.Interfaces;

public interface IFirebaseProvider
{
    Task<NotificationResult> SendAsync(FirebaseNotification firebaseNotification);
}