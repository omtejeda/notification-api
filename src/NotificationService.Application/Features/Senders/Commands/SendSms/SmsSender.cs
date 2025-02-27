using NotificationService.Domain.Enums;
using NotificationService.Domain.Entities;
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Application.Contracts.Persistence;
using NotificationService.Application.Interfaces;
using NotificationService.Application.Features.Senders.Dtos;
using NotificationService.SharedKernel.Interfaces;
using NotificationService.Application.Common.Helpers;
using NotificationService.Application.Features.Providers.Interfaces;
using NotificationService.Domain.Models;
using NotificationService.Application.Common.Models;

namespace NotificationService.Application.Features.Senders.Commands.SendSms;

public class SmsSender(
    INotificationsService notificationsService,
    IRepository<Provider> providerRepository,
    IHttpClientProvider httpClientProvider,
    ITemplateService templateService,
    IDateTimeService dateTimeService,
    IEnvironmentService environmentService) : ISmsSender
{
    private readonly INotificationsService _notificationsService = notificationsService;
    private readonly IRepository<Provider> _providerRepository = providerRepository;
    private readonly IHttpClientProvider _httpClientProvider = httpClientProvider;
    private readonly ITemplateService _templateService = templateService;
    private readonly IDateTimeService _dateTimeService = dateTimeService;
    private readonly IEnvironmentService _environmentService = environmentService;

    public async Task<BaseResponse<NotificationSentResponseDto>> SendSmsAsync(SendSmsRequestDto request, string owner)
    {
        var runtimeTemplate = await _templateService.GetRuntimeTemplate(
            name: request.Template.Name,
            platformName: request.Template.PlatformName,
            language: request.Template.Language,
            providedMetadata: request.Template?.Metadata?.ToList(),
            owner: owner,
            notificationType: NotificationType.SMS);

        var provider = await _providerRepository.FindOneAsync(x => x.Name == request.ProviderName);
        
        Guard.ProviderIsNotNull(provider, request.ProviderName);
        Guard.ProviderIsSuitable(provider.Type, ProviderType.HttpClient);
        Guard.CanSendToDestination(provider, request.ToPhoneNumber, _environmentService.CurrentEnvironment);
        
        ArgumentNullException.ThrowIfNull(provider?.Settings?.HttpClient, nameof(provider.Settings.HttpClient));
        ArgumentNullException.ThrowIfNull(request?.Template?.Metadata, nameof(request.Template.Metadata));

        NotificationResult notificationResult = await _httpClientProvider
            .SendHttpClient(httpClientSetting: provider.Settings.HttpClient,
                templateContent: runtimeTemplate.Content, 
                requestMetadata: request.Template.Metadata,
                requestToDestination: request.ToPhoneNumber);
        
        var notification = Notification.Builder
                .NewNotification()
                .OfType(NotificationType.SMS)
                .To(request.ToPhoneNumber)
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