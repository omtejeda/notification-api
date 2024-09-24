using Microsoft.AspNetCore.Http;
using NotificationService.Application.Features.Senders.Dtos;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Application.Common.Models;

namespace NotificationService.Application.Interfaces;

/// <summary>
/// Interface for sending emails.
/// Provides methods to send emails with optional attachments.
/// </summary>
public interface IEmailSender
{
    /// <summary>
    /// Sends an email based on the specified request parameters.
    /// </summary>
    /// <param name="request">The details of the email to be sent, including recipient, provider and template.</param>
    /// <param name="owner">The identifier of the platform requesting the email to be sent.</param>
    /// <param name="attachments">Optional list of attachments to include with the email.</param>
    /// <returns>A task that represents the asynchronous operation, containing the response with details of the sent notification.</returns>
    Task<BaseResponse<NotificationSentResponseDto>> SendEmailAsync(SendEmailRequestDto request, string owner, List<IFormFile>? attachments = null);
}