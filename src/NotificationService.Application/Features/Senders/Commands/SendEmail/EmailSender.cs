using Microsoft.AspNetCore.Http;
using NotificationService.Domain.Enums;
using NotificationService.Domain.Entities;
using NotificationService.Application.Contracts.Services;
using NotificationService.Application.Contracts.DTOs.Responses;
using NotificationService.Application.Contracts.Senders;
using NotificationService.Application.Features.Senders.Dtos;
using NotificationService.Domain.Models;
using NotificationService.SharedKernel.Interfaces;
using NotificationService.Application.Features.Providers.Interfaces;
using NotificationService.Domain.ValueObjects;
using NotificationService.Application.Common.Models;
using NotificationService.Application.Common.Helpers;

namespace NotificationService.Application.Features.Senders.Commands.SendEmail;

public class EmailSender(
    ITemplateService templateService,
    INotificationsService notificationsService,
    IEmailProviderFactory emailProviderFactory,
    IDateTimeService dateTimeService) : IEmailSender
{
    private readonly ITemplateService _templateService = templateService;
    private readonly INotificationsService _notificationsService = notificationsService;
    private readonly IEmailProviderFactory _emailProviderFactory = emailProviderFactory;
    private readonly IDateTimeService _dateTimeService = dateTimeService;

    public async Task<BaseResponse<NotificationSentResponseDto>> SendEmailAsync(SendEmailRequestDto request, string owner, List<IFormFile>? attachments = null)
    {
        var runtimeTemplate = await _templateService.GetRuntimeTemplate(
            name: request.Template.Name,
            platformName: request.Template.PlatformName,
            language: request.Template.Language,
            providedMetadata: request.Template?.Metadata?.ToList(),
            owner: owner,
            notificationType: NotificationType.Email);

        var attachmentsCollection = attachments.GetCollection().ToList();

        var notification = Notification.Builder
                .NewNotification()
                .OfType(NotificationType.Email)
                .To(request.ToEmail)
                .WithProviderName(request.ProviderName)
                .WithAttachments(attachmentsCollection)
                .CreatedBy(owner)
                .WithRuntimeTemplate(runtimeTemplate)
                .WithUserRequest(request)
                .HasParentNotificationId(request.ParentNotificationId)
                .WithDate(_dateTimeService.UtcToLocalTime)
                .Build();

        IEmailProvider emailProvider = await _emailProviderFactory.CreateProviderAsync(providerName: request.ProviderName, createdBy: owner);

        var emailMessage = EmailMessage.Builder
            .NewMessage()
            .To(request.ToEmail)
            .WithCc(request.CcEmails.Select(Email.From))
            .WithBcc(request.BccEmails.Select(Email.From))
            .WithSubject(runtimeTemplate.Subject)
            .WithContent(runtimeTemplate.Content)
            .AddHeader(EmailHelper.Parameters.NotificationIdHeader,notification.NotificationId)
            .UsingMetadata(runtimeTemplate.ProvidedMetadata)
            .WithAttachments(attachmentsCollection)
            .Build();

        NotificationResult notificationResult = await emailProvider.SendAsync(emailMessage);
        notification.AddNotificationResult(notificationResult);

        await _notificationsService.RegisterNotification(notification);

        if (notification.MustSaveAttachments)
            await _notificationsService.SaveAttachments(attachmentsCollection);
        
        return new BaseResponse<NotificationSentResponseDto>(
            code: notificationResult.Code,
            message: notificationResult.Message,
            data: new NotificationSentResponseDto
            {
                NotificationId = notification.NotificationId
            });
    }
}