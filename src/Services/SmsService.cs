using System;
using System.Linq;
using System.Threading.Tasks;
using NotificationService.Dtos;
using NotificationService.Dtos.Requests;
using NotificationService.Utils;
using NotificationService.Repositories;
using NotificationService.Enums;
using NotificationService.Entities;
using NotificationService.Dtos.Responses;
using NotificationService.Exceptions;
using NotificationService.Services.Interfaces;
using static NotificationService.Utils.SystemUtil;

namespace NotificationService.Services
{
    public class SmsService : ISmsService
    {
        private readonly IRepository<Template> _templateRepository;
        private readonly INotificationsService _notificationsService;
        private readonly IRepository<Provider> _providerRepository;
        private readonly IHttpClientService _httpClientService;

        public SmsService(IRepository<Template> templateRepository, INotificationsService notificationsService, IRepository<Provider> providerRepository, IHttpClientService httpClientService)
        {
            _templateRepository = templateRepository;
            _notificationsService = notificationsService;
            _providerRepository = providerRepository;
            _httpClientService = httpClientService;
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
            var (templates, _) = await _templateRepository.FindAsync(t => t.Name == request.Template.Name && t.PlatformName == request.Template.PlatformName);
            var templateObj = templates.FirstOrDefault(x => x.Language == request.Template.Language);
            
            if (templateObj == null)
                throw new RuleValidationException("Couldn't find template");

            if (templateObj.NotificationType != NotificationType.SMS)
                throw new RuleValidationException($"Template specified does not correspond to {NotificationType.SMS.ToString()}. It corresponds to {templateObj.NotificationType.ToString()}");

            if (templateObj?.Content == null)
                throw new RuleValidationException("Template provided does not have content");

            var provider = await _providerRepository.FindOneAsync(x => x.Name == request.ProviderName);
            if (provider == null)
                throw new RuleValidationException($"Provider {request.ProviderName} does not exist");

            if (provider.Type != ProviderType.HttpClient)
                throw new RuleValidationException($"No suitable provider found to perform this action. Current: {provider.Type.ToString()}");

            ThrowIfPhoneNotAllowed(toPhoneNumber: request.ToPhoneNumber, provider: provider);

            var content = templateObj?.Content;
            var smsContent = EmailUtil.ReplaceParameters(content, metadata: request.Template?.Metadata);
            
            var (success, code, message) = await _httpClientService.SendHttpClient(provider.Settings?.HttpClient.Host, provider.Settings?.HttpClient.Uri, provider.Settings?.HttpClient.Verb, provider.Settings?.HttpClient.Params, smsContent, request.Template.Metadata, requestToDestination: request.ToPhoneNumber);
            var notificationId = await _notificationsService.RegisterNotification(NotificationType.SMS, toDestination: request.ToPhoneNumber, templateName: templateObj.Name, platformName: templateObj.PlatformName, providerName: request.ProviderName, success: success, message: message, owner: owner, request: request, request.ParentNotificationId);
            
            return new FinalResponseDTO<NotificationSentResponseDto>(code, message, new NotificationSentResponseDto { NotificationId = notificationId });
        }
    }
}