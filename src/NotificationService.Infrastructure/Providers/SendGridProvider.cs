using SendGrid.Helpers.Mail;
using SendGrid;
using NotificationService.Domain.Enums;
using NotificationService.Application.Utils;
using NotificationService.Domain.Entities;
using NotificationService.Application.Features.Senders.Models;
using NotificationService.Domain.Models;
using NotificationService.Common.Interfaces;
using NotificationService.Application.Features.Providers.Interfaces;

namespace NotificationService.Infrastructure.Providers;

public class SendGridProvider(IEnvironmentService environmentService) : IEmailProvider
{
    public ProviderType ProviderType => ProviderType.SendGrid;
    private Provider _provider;

    private readonly IEnvironmentService _environmentService = environmentService;

    public void SetProvider(Provider provider)
    {
        _provider = provider;
    }

    public async Task<NotificationResult> SendAsync(EmailMessage emailMessage)
    {
        EmailUtil.ThrowIfEmailNotAllowed(
            environment: _environmentService.CurrentEnvironment,
            provider: _provider,
            to: emailMessage.To,
            cc: emailMessage.Cc,
            bcc: emailMessage.Bcc);

        ThrowIfSettingsNotValid();

        var sendGridTemplate = EmailUtil.GetSendgridTemplateFromMetadata(emailMessage.ProvidedMetadata);

        var client = new SendGridClient(_provider.Settings.SendGrid.ApiKey);
        var msg = new SendGridMessage()
        {
            From = new EmailAddress(_provider.Settings.SendGrid.FromEmail, _provider.Settings.SendGrid.FromDisplayName),
            Headers = emailMessage.Headers
        };

        if (sendGridTemplate.HasTemplate)
        {
            msg.TemplateId = sendGridTemplate.TemplateId;
            msg.Categories = new List<string> { sendGridTemplate.Category };
            msg.SetTemplateData(sendGridTemplate.DynamicTemplateData);
        }
        else
        {
            msg.Subject = emailMessage.Subject;
            msg.HtmlContent = emailMessage.Content;
        }

        msg.AddTo(new EmailAddress(emailMessage.To));

        emailMessage.Cc?.ForEach(ccEmail => { msg.AddCc(new EmailAddress(ccEmail)); });
        emailMessage.Bcc?.ForEach(bccEmail => { msg.AddBcc(new EmailAddress(bccEmail)); });

        var attachments = emailMessage?.Attachments?.Select(x => x.FormFile).ToList();

        msg.AddAttachments(attachments);

        var response = await client.SendEmailAsync(msg);

        if (!response.IsSuccessStatusCode)
        {
            return NotificationResult.Fail(
                code: (int)ResultCode.EmailNotSent,
                message: $"Something went wrong when trying to send email: {response.StatusCode} {response?.ToString()}");
        }

        return NotificationResult.Ok(
            code: (int)ResultCode.OK,
            message: "Email queued successfully using SendGrid!",
            from: _provider.Settings.SendGrid.FromEmail,
            savesAttachments: _provider.SavesAttachments);
    }

    private void ThrowIfSettingsNotValid()
    {
        if (string.IsNullOrWhiteSpace(_provider?.Settings?.SendGrid?.FromEmail))
            throw new ArgumentNullException(nameof(_provider.Settings.SendGrid.FromEmail));
        
        if (string.IsNullOrWhiteSpace(_provider?.Settings?.SendGrid?.ApiKey))
            throw new ArgumentNullException(nameof(_provider.Settings.SendGrid.ApiKey));
    }
}