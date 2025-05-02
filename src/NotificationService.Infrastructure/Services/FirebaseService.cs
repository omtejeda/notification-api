using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Models;
using NotificationService.Infrastructure.Interfaces;
using NotificationService.Infrastructure.Models;

namespace NotificationService.Infrastructure.Services;

internal class FirebaseService : IFirebaseService
{
    private static readonly Dictionary<string, FirebaseApp> _firebaseApps = [];
    public async Task<IReadOnlyList<SendResponse>> SendNotificationAsync(FirebaseNotification firebaseNotification, CancellationToken cancellationToken = default)
    {
        var firebaseMessaging = GetFirebaseMessaging(firebaseNotification.FirebaseSetting);
        var multicastMessage = GetMulticastMessage(firebaseNotification);
        
        var result = await firebaseMessaging.SendEachForMulticastAsync(multicastMessage, cancellationToken);
        return result.Responses;
    }

    private static FirebaseMessaging GetFirebaseMessaging(FirebaseSetting firebaseSetting)
    {
        var jsonCredentials = FirebaseCredential
            .ConvertFrom(firebaseSetting)
            .ToJson();

        var appName = firebaseSetting.ProjectId;

        if (!_firebaseApps.TryGetValue(appName, out var app))
        {
            app = FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromJson(jsonCredentials)
            }, appName);

            _firebaseApps[appName] = app;
        }

        return FirebaseMessaging.GetMessaging(app);
    }

    private static MulticastMessage GetMulticastMessage(FirebaseNotification firebaseNotification)
    {
        return new()
        {
            Notification = new()
            {
                Title = firebaseNotification.Title,
                Body = firebaseNotification.Body
            },
            Tokens = firebaseNotification.UserTokens
        };
    }
}