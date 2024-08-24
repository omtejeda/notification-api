using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using NotificationService.Core.Common.Enums;
using NotificationService.Core.Common.Entities;
using NotificationService.Core.Common.Utils;
using NotificationService.Contracts.Interfaces.Services;
using NotificationService.Core.Providers.Factories.Interfaces;
using NotificationService.Core.Common;
using NotificationService.Contracts.RequestDtos;
using NotificationService.Contracts.ResponseDtos;
using NotificationService.Contracts.Interfaces.Senders;
using NotificationService.Core.Providers.Interfaces;

namespace NotificationService.Core.Senders
{
    public class EmailSender : IEmailSender
    {
        private readonly ITemplateService _templateService;
        private readonly INotificationsService _notificationsService;
        private readonly IEmailProviderFactory _emailProviderFactory;

        public EmailSender(
            ITemplateService templateService, 
            INotificationsService notificationsService,
            IEmailProviderFactory emailProviderFactory)
        {
            _templateService = templateService;
            _notificationsService = notificationsService;
            _emailProviderFactory = emailProviderFactory;
        }

        public async Task<FinalResponseDTO<NotificationSentResponseDto>> SendEmailAsync(SendEmailRequestDto request, string owner, List<IFormFile> attachments = null)
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

            if (notification.AppliesSavesAttachments)
            {
                await _notificationsService.SaveAttachments(attachmentsCollection);
            }
            
            return new FinalResponseDTO<NotificationSentResponseDto>(notificationResult.Code, notificationResult.Message, new NotificationSentResponseDto { NotificationId = notification.NotificationId });
        }
    }
}