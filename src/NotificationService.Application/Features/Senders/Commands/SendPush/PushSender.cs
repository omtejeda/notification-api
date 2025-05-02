using NotificationService.Application.Common.Helpers;
using NotificationService.Application.Common.Models;
using NotificationService.Application.Contracts.DTOs.Responses;
using NotificationService.Application.Contracts.Persistence;
using NotificationService.Application.Contracts.Senders;
using NotificationService.Application.Contracts.Services;
using NotificationService.Application.Features.Providers.Interfaces;
using NotificationService.Application.Features.Senders.Dtos;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Enums;
using NotificationService.Domain.Models;
using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Senders.Commands.SendPush;

public class PushSender(
    INotificationsService notificationsService,
    ITemplateService templateService,
    IRepository<Provider> providerRepository,
    IDateTimeService dateTimeService,
    IEnvironmentService environmentService,
    IFirebaseProvider firebaseProvider) : IPushSender
{
    private readonly INotificationsService _notificationsService = notificationsService;
    private readonly ITemplateService _templateService = templateService;
    private readonly IRepository<Provider> _providerRepository = providerRepository;
    private readonly IDateTimeService _dateTimeService = dateTimeService;
    private readonly IEnvironmentService _environmentService = environmentService;
    private readonly IFirebaseProvider _firebaseProvider = firebaseProvider;
    
    public async Task<BaseResponse<NotificationSentResponseDto>> SendPushAsync(SendPushRequestDto request, string owner)
    {
        var runtimeTemplate = await _templateService.GetRuntimeTemplate(
            name: request.Template.Name,
            platformName: request.Template.PlatformName,
            language: request.Template.Language,
            providedMetadata: request.Template?.Metadata?.ToList(),
            owner: owner,
            notificationType: NotificationType.Push);

        var provider = await _providerRepository.FindOneAsync(x => x.Name == request.ProviderName);

        Guard.ProviderIsNotNull(provider, request.ProviderName);
        Guard.ProviderIsSuitable(provider.Type, ProviderType.Firebase);
        Guard.CanSendToDestination(provider, request.UserId, _environmentService.CurrentEnvironment);

        var firebaseNotification = new FirebaseNotification
        {
            UserTokens = request.UserTokens,
            Title = runtimeTemplate.Subject,
            Body = runtimeTemplate.Content,
            FirebaseSetting = provider?.Settings?.Firebase ?? new()
        };

        NotificationResult notificationResult = await _firebaseProvider.SendAsync(firebaseNotification);

        var notification = Notification.Builder
            .NewNotification()
            .OfType(NotificationType.Push)
            .To(request.UserId)
            .WithProviderName(request.ProviderName)
            .HasParentNotificationId(request.ParentNotificationId)
            .WithRuntimeTemplate(runtimeTemplate)
            .WithUserRequest(request)
            .CreatedBy(owner)
            .WithDate(_dateTimeService.UtcToLocalTime)
            .Build();

        notification.AddNotificationResult(notificationResult);

        await _notificationsService.RegisterNotification(notification);

        return new BaseResponse<NotificationSentResponseDto>(
            notificationResult.Code,
            notificationResult.Message,
            new NotificationSentResponseDto
            {
                NotificationId = notification.NotificationId
            });
    }
}