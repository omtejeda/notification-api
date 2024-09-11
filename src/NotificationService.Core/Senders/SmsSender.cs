using System;
using System.Linq;
using System.Threading.Tasks;
using NotificationService.Domain.Enums;
using NotificationService.Domain.Entities;
using NotificationService.Core.Contracts.Interfaces.Services;
using NotificationService.Core.Contracts.ResponseDtos;
using NotificationService.Core.Contracts.Interfaces.Repositories;
using NotificationService.Core.Interfaces;
using NotificationService.Core.Providers.Interfaces;
using NotificationService.Common.Dtos;
using NotificationService.Core.Dtos;
using NotificationService.Common.Utils;
using NotificationService.Common.Interfaces;

namespace NotificationService.Core.Senders
{
    public class SmsSender : ISmsSender
    {
        private readonly INotificationsService _notificationsService;
        private readonly IRepository<Provider> _providerRepository;
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly ITemplateService _templateService;
        private readonly IDateTimeService _dateTimeService;
        private readonly IEnvironmentService _environmentService;

        public SmsSender(
            INotificationsService notificationsService,
            IRepository<Provider> providerRepository,
            IHttpClientProvider httpClientProvider,
            ITemplateService templateService,
            IDateTimeService dateTimeService,
            IEnvironmentService environmentService)
        {
            _notificationsService = notificationsService;
            _providerRepository = providerRepository;
            _httpClientProvider = httpClientProvider;
            _templateService = templateService;
            _dateTimeService = dateTimeService;
            _environmentService = environmentService;
        }

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

            var (success, code, message) = await _httpClientProvider
                .SendHttpClient(httpClientSetting: provider?.Settings?.HttpClient,
                    templateContent: runtimeTemplate.Content, 
                    requestMetadata: request.Template.Metadata,
                    requestToDestination: request.ToPhoneNumber);
            
            var notification = Notification.Builder
                    .NewNotification()
                    .OfType(NotificationType.SMS)
                    .From(provider?.Settings?.HttpClient.Host)
                    .To(request.ToPhoneNumber)
                    .WithProviderName(request.ProviderName)
                    .HasParentNotificationId(request.ParentNotificationId)
                    .WithRuntimeTemplate(runtimeTemplate)
                    .WithUserRequest(request)
                    .WasSuccess(success)
                    .WithResultMessage(message)
                    .CreatedBy(owner)
                    .WithDate(_dateTimeService.UtcToLocalTime)
                    .Build();

            await _notificationsService.RegisterNotification(notification);
            
            return new BaseResponse<NotificationSentResponseDto>(code, message, new NotificationSentResponseDto { NotificationId = notification.NotificationId });
        }
    }
}