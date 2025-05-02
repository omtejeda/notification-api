using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Features.Providers.Interfaces;
using NotificationService.Domain.Enums;
using NotificationService.Domain.Models;
using NotificationService.Infrastructure.Interfaces;

namespace NotificationService.Infrastructure.Providers;

internal class FirebaseProvider(IFirebaseService firebaseService, ILogger<FirebaseProvider> logger) : IFirebaseProvider
{
    private readonly IFirebaseService _firebaseService = firebaseService;
    private readonly ILogger _logger = logger;
    
    public async Task<NotificationResult> SendAsync(FirebaseNotification firebaseNotification)
    {
        try
        {
            var results = await _firebaseService.SendNotificationAsync(firebaseNotification);
            var success = results.Any(x => x.IsSuccess);
            
            return success
                ? NotificationResult.Ok((int)ResultCode.OK, $"Sent successfully using Firebase. MessageId: {GetMessagesId(results)}")
                : NotificationResult.Fail((int)ResultCode.FirebaseNotificationFailed, $"Failed to send. {GetExceptionMessages(results)}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error ocurred trying to interact with Firebase: {Message}", e.Message);
            
            var errorMsg = $"An error ocurred trying to send notification with Firebase: {e.Message}";
            return NotificationResult.Fail((int)ResultCode.FirebaseNotificationFailed, errorMsg);
        }
    }

    private static string GetMessagesId(IReadOnlyList<SendResponse> sendResponses) =>
        string.Join(", ", sendResponses.Select(x => x.MessageId));

    private static string GetExceptionMessages(IReadOnlyList<SendResponse> sendResponses)
    {
        var results = sendResponses
            .Where(x => x.Exception != null)
            .Select(x =>
            {
                return x.Exception is FirebaseMessagingException fme
                    ? $"Message: {fme.Message}, Code: {fme.MessagingErrorCode}, StatusCode: {fme.HttpResponse?.StatusCode}"
                    : $"Message: {x.Exception.Message}";
            });

        return string.Join(", ", results);
    }   
}