using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using NotificationService.Dtos;
using NotificationService.Interfaces;
using NotificationService.Dtos.Requests;
using NotificationService.Utils;
using NotificationService.Repositories;
using NotificationService.Enums;
using NotificationService.Entities;
using NotificationService.Dtos.Responses;
using NotificationService.Exceptions;
using NotificationService.Services.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using MimeKit;
using MailKit.Net.Smtp;
using static NotificationService.Utils.SystemUtil;

namespace NotificationService.Services
{
    public class EmailService : IEmailService
    {
        private readonly IRepository<Template> _templateRepository;
        private readonly IRepository<Provider> _providerRepository;
        private readonly INotificationsService _notificationsService;
        
        private readonly IHttpClientService _httpClientService;

        public EmailService(IRepository<Template> templateRepository, IRepository<Provider> providerRepository, INotificationsService notificationsService, IHttpClientService httpClientService)
        {
            _templateRepository = templateRepository;
            _providerRepository = providerRepository;
            _notificationsService = notificationsService;
            _httpClientService = httpClientService;
        }

        private void ThrowIfEmailNotAllowed(string toEmail, Provider provider)
        {
            if (IsProduction()) return;

            var isEmailAllowed = provider?.DevSettings?.AllowedRecipients?.Any(x => x.ToLower() == toEmail.ToLower()) ?? false;
            if (!isEmailAllowed)
            {
                throw new RuleValidationException($"Not allowed sending to {toEmail} in non production environment");
            }
        }

        public async Task<FinalResponseDTO<NotificationSentResponseDto>> SendEmailAsync(SendEmailRequestDto request, string owner, List<IFormFile> attachments = null)
        {
            var (templates, _) = await _templateRepository.FindAsync(t => t.Name == request.Template.Name && t.PlatformName == request.Template.PlatformName);
            var templateObj = templates.FirstOrDefault(x => x.Language == request.Template.Language);
            
            if (templateObj == null)
                throw new RuleValidationException("Couldn't find template");
            
            if (templateObj.CreatedBy != owner && templateObj.PlatformName != owner)
                throw new RuleValidationException("Template does not belong to your platform!");

            if (templateObj.NotificationType != NotificationType.Email)
                throw new RuleValidationException($"Template specified does not correspond to {NotificationType.Email.ToString()}. It corresponds to {templateObj.NotificationType.ToString()}");

            if (templateObj?.Content == null)
                throw new RuleValidationException("Template provided does not have content");

            var provider = await _providerRepository.FindOneAsync(x => x.Name == request.ProviderName);
            if (provider == null)
                throw new RuleValidationException($"Provider {request.ProviderName} does not exist");
            
            if (!(provider.IsPublic ?? false) &&  provider.CreatedBy != owner)
                throw new RuleValidationException($"Provider provided is not public. It wasn't created by you either!");

            ThrowIfEmailNotAllowed(toEmail: request.ToEmail, provider: provider);

            var mailHtml = templateObj?.Content;
            var htmlBody = EmailUtil.ReplaceParameters(mailHtml, metadata: request.Template?.Metadata);

            var success = false;
            var code = (int) ErrorCode.EmailPending;
            var message = string.Empty;
            
            if (provider.Type == ProviderType.SMTP)
            {
                (success, code, message) = await SendSMTP(request.ToEmail, provider.Settings?.Smtp.FromEmail, templateObj.Subject, htmlBody, provider.Settings?.Smtp.Host, provider.Settings?.Smtp.Port, provider.Settings?.Smtp.Password, attachments, provider.Settings?.Smtp.Authenticate );
            }
            else if (provider.Type == ProviderType.SendGrid)
            {
                (success, code, message) = await SendSendGrid(request.ToEmail, provider.Settings?.SendGrid.FromEmail, provider.Settings?.SendGrid.FromDisplayName, templateObj.Subject, htmlBody, provider.Settings?.SendGrid.ApiKey, attachments);
            }
            else if (provider.Type == ProviderType.HttpClient)
            {
                (success, code, message) = await _httpClientService.SendHttpClient(provider.Settings.HttpClient.Host, provider.Settings.HttpClient.Uri, provider.Settings.HttpClient.Verb, provider.Settings.HttpClient.Params, templateObj.Content, request.Template.Metadata, request.ToEmail);
            }
            else
            {
                throw new RuleValidationException("No provider found to perform this action");
            }

            var notificationId = await _notificationsService
                    .RegisterNotification(NotificationType.Email, toDestination: request.ToEmail, templateName: templateObj.Name, platformName: templateObj.PlatformName, providerName: request.ProviderName, success: success, message: message, owner: owner, request: request, request.ParentNotificationId, attachments: attachments);
            
            return new FinalResponseDTO<NotificationSentResponseDto>(code, message, new NotificationSentResponseDto { NotificationId = notificationId });
        }



        private async Task<Tuple<bool, int, string>> SendSendGrid(string toEmail, string fromEmail, string fromDisplayName, string subject, string htmlContent, string apiKey, List<IFormFile> attachments = null)
        {
            EmailUtil.CheckSendGridSettings(fromEmail, apiKey);

            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(fromEmail, fromDisplayName),
                Subject = subject,
                HtmlContent = htmlContent
            };

            msg.AddTo(new EmailAddress(toEmail));
            msg.AddAttachments(attachments);

            var response = await client.SendEmailAsync(msg);

            var code = (int) ErrorCode.OK;
            var message = "Email queued successfully using SendGrid!";

            var success = response.IsSuccessStatusCode;
            if (!success)
            {
                code = (int) ErrorCode.EmailNotSent;
                message = $"Something went wrong when trying to send email: {response.StatusCode}";
            }

            return (success, code, message).ToTuple();
        }

        private async Task<Tuple<bool, int, string>> SendSMTP(string toEmail, string fromEmail, string subject, string htmlContent, string host, int? port, string password, List<IFormFile> attachments = null, bool? authenticate = false)
        {
            EmailUtil.CheckSMTPSettings(fromEmail, host, port, password);
            try
            {
                var builder = new BodyBuilder { HtmlBody = htmlContent };
                builder.AddAttachments(attachments);

                var email = new MimeMessage
                {
                    Sender = MailboxAddress.Parse(fromEmail),
                    Subject =  subject,
                    Body = builder.ToMessageBody()
                };

                email.To.Add(MailboxAddress.Parse(toEmail));

                using var smtp = new SmtpClient();
                smtp.Connect(host, (int) port, MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable);
                
                if (authenticate ?? false)
                    smtp.Authenticate(fromEmail, password);

                await smtp.SendAsync(email);
                smtp.Disconnect(true);

                return (success: true, code: (int) ErrorCode.OK, message: "Email sent successfully using SMTP").ToTuple();
            }
            catch (Exception e)
            {
                return (success: false, code: (int) ErrorCode.EmailNotSent, message: $"Something went wrong when trying to send email: {e.Message}").ToTuple();
            }
        }
    }
}