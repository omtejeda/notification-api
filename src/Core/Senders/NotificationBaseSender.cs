using System;
using System.Linq;
using System.Threading.Tasks;
using NotificationService.Core.Common.Enums;
using NotificationService.Core.Common.Entities;
using NotificationService.Core.Common.Exceptions;
using static NotificationService.Core.Common.Utils.SystemUtil;
using NotificationService.Contracts.Interfaces.Services;
using NotificationService.Core.Providers.Entities;
using NotificationService.Core.Providers.Enums;
using NotificationService.Contracts.RequestDtos;
using NotificationService.Contracts.ResponseDtos;
using NotificationService.Contracts.Interfaces.Repositories;
using NotificationService.Contracts.Interfaces.Senders;
using NotificationService.Core.Providers.Interfaces;
using NotificationService.Core.Templates.Models;


namespace NotificationService.Core.Senders
{
    public abstract class NotificationBaseSender
    {
        private readonly ITemplateService _templateService;
        private readonly IRepository<Provider> _providerRepository;
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly INotificationsService _notificationsService;

        public NotificationType NotificationType { get; protected set; }
        public string ToDestination { get; protected set; } = string.Empty;


        public NotificationBaseSender(ITemplateService templateService, 
            IRepository<Provider> providerRepository, 
            IHttpClientProvider httpClientProvider,
            INotificationsService notificationsService)
        {
            _templateService = templateService;
            _providerRepository = providerRepository;
            _httpClientProvider = httpClientProvider;
            _notificationsService = notificationsService;
        }

        protected abstract void Setup(NotificationBaseRequestDto message);
        protected abstract void InitialCheck(NotificationBaseRequestDto message);


        public async Task<FinalResponseDTO<NotificationSentResponseDto>> SendAsync(NotificationBaseRequestDto basicMessage, string owner)
        {
            Setup(basicMessage);
            Guards();
            InitialCheck(basicMessage);

            RuntimeTemplate runtimeTemplate = await GetRuntimeTemplateAsync(basicMessage, owner);
            Provider provider = await GetProviderAsync(basicMessage.ProviderName);

            ValidateProvider(provider);
            ValidateDestination(toDestination: ToDestination, provider: provider);

            var (success, code, message) = await SendViaHttpClientAsync(basicMessage, runtimeTemplate, provider);

            Notification notification = BuildNotification(basicMessage, owner, runtimeTemplate, provider, success, message);
            await RegisterNotificationAsync(notification);

            return CreateResponse(code, message, notification);
        }

        protected virtual async Task<Tuple<bool, int, string>> SendViaHttpClientAsync(NotificationBaseRequestDto basicMessage, RuntimeTemplate runtimeTemplate, Provider provider)
        {
            return await _httpClientProvider
                .SendHttpClient(httpClientSetting: provider?.Settings?.HttpClient,
                    templateContent: runtimeTemplate.Content,
                    requestMetadata: basicMessage.Template.Metadata,
                    requestToDestination: ToDestination);
        }

        private static FinalResponseDTO<NotificationSentResponseDto> CreateResponse(int code, string message, Notification notification)
        {
            return new FinalResponseDTO<NotificationSentResponseDto>(code, message, new NotificationSentResponseDto { NotificationId = notification.NotificationId });
        }

        private async Task RegisterNotificationAsync(Notification notification)
        {
            await _notificationsService.RegisterNotification(notification);
        }

        protected virtual Notification BuildNotification(NotificationBaseRequestDto basicMessage, string owner, RuntimeTemplate runtimeTemplate, Provider provider, bool success, string message)
        {
            var notification = Notification.Builder
                    .NewNotification()
                    .OfType(NotificationType)
                    .From(provider?.Settings?.HttpClient.Host)
                    .To(ToDestination)
                    .WithProviderName(basicMessage.ProviderName)
                    .HasParentNotificationId(basicMessage.ParentNotificationId)
                    .WithRuntimeTemplate(runtimeTemplate)
                    // .WithUserRequest(request)
                    .WasSuccess(success)
                    .WithResultMessage(message)
                    .CreatedBy(owner)
                    .Build();
            return notification;
        }

        protected virtual async Task<Provider> GetProviderAsync(string providerName)
        {
            return await _providerRepository.FindOneAsync(x => x.Name == providerName)
                ?? throw new RuleValidationException($"Provider does not exist");
        }

        protected virtual async Task<RuntimeTemplate> GetRuntimeTemplateAsync(NotificationBaseRequestDto basicMessage, string owner)
        {
            return await _templateService.GetRuntimeTemplate(
                name: basicMessage.Template.Name,
                platformName: basicMessage.Template.PlatformName,
                language: basicMessage.Template.Language,
                providedMetadata: basicMessage.Template?.Metadata?.ToList(),
                owner: owner,
                notificationType: NotificationType);
        }

        protected virtual void ValidateProvider(Provider provider)
        {
            if (provider.Type != ProviderType.HttpClient)
                throw new RuleValidationException($"No suitable provider found to perform this action. Current: {provider.Type}");
        }

        protected virtual void ValidateDestination(string toDestination, Provider provider)
        {
            if (IsProduction()) return;

            var isDestinationAllowed = provider?.DevSettings?.AllowedRecipients?.Any(x => x == toDestination) ?? false;
            if (!isDestinationAllowed)
            {
                throw new RuleValidationException($"Not allowed sending to {toDestination} in non production environment");
            }
        }

        private void Guards()
        {
            ToDestinationGuard();
            NotificationTypeGuard();
        }

        private void NotificationTypeGuard()
        {
            if (NotificationType == NotificationType.None)
                throw new ArgumentException("Could not determine Notification Type");
        }

        private void ToDestinationGuard()
        {
            if (string.IsNullOrWhiteSpace(ToDestination))
                throw new ArgumentException("Could not determine Destination");
        }
    }


    public class ConcreteSmsSender : NotificationBaseSender
    {
        public ConcreteSmsSender(
            ITemplateService templateService, 
            IRepository<Provider> providerRepository, 
            IHttpClientProvider httpClientProvider,
            INotificationsService notificationsService) 
            : base(templateService, providerRepository, httpClientProvider, notificationsService)
        {

        }

        protected override void Setup(NotificationBaseRequestDto message)
        {
            NotificationType = NotificationType.SMS;

            var smsMessage = message as SendSmsRequestDto;
            ToDestination = smsMessage.ToPhoneNumber;
        }

        protected override void InitialCheck(NotificationBaseRequestDto message)
        {
        }
    }
}

