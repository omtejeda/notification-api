using System;
using System.Linq;
using System.Threading.Tasks;
using NotificationService.Common.Enums;
using NotificationService.Common.Entities;
using NotificationService.Core.Common.Exceptions;
using static NotificationService.Common.Utils.SystemUtil;
using NotificationService.Contracts.Interfaces.Services;
using NotificationService.Contracts.ResponseDtos;
using NotificationService.Contracts.Interfaces.Repositories;
using NotificationService.Core.Interfaces;
using NotificationService.Core.Providers.Interfaces;
using NotificationService.Common.Dtos;
using NotificationService.Core.Dtos;

namespace NotificationService.Core.Senders
{
    public class SmsSender : ISmsSender
    {
        private readonly INotificationsService _notificationsService;
        private readonly IRepository<Provider> _providerRepository;
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly ITemplateService _templateService;

        public SmsSender(INotificationsService notificationsService, IRepository<Provider> providerRepository, IHttpClientProvider httpClientProvider, ITemplateService templateService)
        {
            _notificationsService = notificationsService;
            _providerRepository = providerRepository;
            _httpClientProvider = httpClientProvider;
            _templateService = templateService;
        }

        private void ThrowIfPhoneNotAllowed(string toPhoneNumber, Provider provider)
        {
            if (IsProduction()) return;

            var isPhoneNumberAllowed = provider?.DevSettings?.AllowedRecipients?.Any(x => x == toPhoneNumber) ?? false;
            if (!isPhoneNumberAllowed)
            {
                throw new RuleValidationException($"Not allowed sending to {toPhoneNumber} in non production environment");
            }
        }

        public async Task<FinalResponseDTO<NotificationSentResponseDto>> SendSmsAsync(SendSmsRequestDto request, string owner)
        {
            var runtimeTemplate = await _templateService.GetRuntimeTemplate(
                name: request.Template.Name,
                platformName: request.Template.PlatformName,
                language: request.Template.Language,
                providedMetadata: request.Template?.Metadata?.ToList(),
                owner: owner,
                notificationType: NotificationType.SMS);

            var provider = await _providerRepository.FindOneAsync(x => x.Name == request.ProviderName);
            
            if (provider is null)
                throw new RuleValidationException($"Provider {request.ProviderName} does not exist");

            if (provider.Type != ProviderType.HttpClient)
                throw new RuleValidationException($"No suitable provider found to perform this action. Current: {provider.Type}");

            ThrowIfPhoneNotAllowed(toPhoneNumber: request.ToPhoneNumber, provider: provider);

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
                    .Build();

            await _notificationsService.RegisterNotification(notification);
            
            return new FinalResponseDTO<NotificationSentResponseDto>(code, message, new NotificationSentResponseDto { NotificationId = notification.NotificationId });
        }
    }
}