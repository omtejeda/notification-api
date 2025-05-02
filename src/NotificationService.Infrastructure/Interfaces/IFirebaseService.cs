using FirebaseAdmin.Messaging;
using NotificationService.Domain.Models;

namespace NotificationService.Infrastructure.Interfaces;

internal interface IFirebaseService
{
    Task<IReadOnlyList<SendResponse>> SendNotificationAsync(FirebaseNotification firebaseNotification, CancellationToken cancellationToken = default);
}