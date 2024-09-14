using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using NotificationService.Domain.Enums;
using NotificationService.Domain.Entities;
using NotificationService.Application.Utils;
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Application.Providers.Factories.Interfaces;
using NotificationService.Application.Common;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Application.Interfaces;
using NotificationService.Application.Providers.Interfaces;
using NotificationService.Common.Dtos;
using NotificationService.Application.Dtos;
using NotificationService.Domain.Models;
using NotificationService.Application.Common.Interfaces;

namespace NotificationService.Application.Senders
{
    public class EmailSender : IEmailSender
    {
        private readonly ITemplateService _templateService;
        private readonly INotificationsService _notificationsService;
        private readonly IEmailProviderFactory _emailProviderFactory;
        private readonly IDateTimeService _dateTimeService;

        public EmailSender(
            ITemplateService templateService, 
            INotificationsService notificationsService,
            IEmailProviderFactory emailProviderFactory,
            IDateTimeService dateTimeService)
        {
            _templateService = templateService;
            _notificationsService = notificationsService;
            _emailProviderFactory = emailProviderFactory;
            _dateTimeService = dateTimeService;
        }

        public async Task<BaseResponse<NotificationSentResponseDto>> SendEmailAsync(SendEmailRequestDto request, string owner, List<IFormFile> attachments = null)
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
                .WithCc(request.CcEmails)
                .WithBcc(request.BccEmails)
                .WithSubject(runtimeTemplate.Subject)
                .WithContent(runtimeTemplate.Content)
                .AddHeader(EmailUtil.Parameters.NotificationIdHeader,notification.NotificationId)
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
}