using NotificationService.Application.Features.Senders.Dtos;
using NotificationService.Application.Contracts.DTOs.Responses;
using NotificationService.Application.Common.Models;

namespace NotificationService.Application.Contracts.Senders;

/// <summary>
/// Interface for sending SMS notifications.
/// Provides methods to send SMS messages to specified recipients.
/// </summary>
public interface IPushSender
{
    /// <summary>
    /// Sends an push based on the specified request parameters.
    /// </summary>
    /// <param name="request">The details of the push to be sent, including recipient, provider and template.</param>
    /// <param name="owner">The identifier of the platform requesting the push notification to be sent.</param>
    /// <returns>A task that represents the asynchronous operation, containing the response with details of the sent notification.</returns>
    Task<BaseResponse<NotificationSentResponseDto>> SendPushAsync(SendPushRequestDto request, string owner);
}