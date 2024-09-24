using NotificationService.Application.Features.Senders.Dtos;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Application.Common.Models;

namespace NotificationService.Application.Interfaces;

/// <summary>
/// Interface for sending messages.
/// Provides methods to send messages to specified recipients.
/// </summary>
public interface IMessageSender
{
    /// <summary>
    /// Sends a message based on the specified request parameters.
    /// </summary>
    /// <param name="request">The details of the message to be sent, including recipient, notification type, provider and template.</param>
    /// <param name="owner">The identifier of the platform requesting the message to be sent.</param>
    /// <returns>A task that represents the asynchronous operation, containing the response with details of the sent notification.</returns>
    Task<BaseResponse<NotificationSentResponseDto>> SendMessageAsync(SendMessageRequestDto request, string owner);
}