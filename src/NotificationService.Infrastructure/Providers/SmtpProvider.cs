using MimeKit;
using MailKit.Net.Smtp;
using NotificationService.Domain.Enums;
using NotificationService.Application.Utils;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Models;
using NotificationService.Common.Interfaces;
using NotificationService.Application.Features.Providers.Interfaces;
using NotificationService.Application.Features.Senders.Models;

namespace NotificationService.Infrastructure.Providers
{
    public class SmtpProvider : IEmailProvider
    {
        public ProviderType ProviderType => ProviderType.SMTP;
        private Provider _provider;
        private readonly TimeSpan _timeout = TimeSpan.FromSeconds(10);

        private readonly IEnvironmentService _environmentService;

        public SmtpProvider(IEnvironmentService environmentService)
        {
            _environmentService = environmentService;
        }

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
            
            try
            {
                var builder = new BodyBuilder { HtmlBody = emailMessage.Content };
                builder.AddAttachments(emailMessage?.Attachments?.Select(x => x.FormFile).ToList());

                var email = new MimeMessage
                {
                    Sender = MailboxAddress.Parse(_provider.Settings.Smtp.FromEmail),
                    Subject = emailMessage.Subject,
                    Body = builder.ToMessageBody()
                };

                email.To.Add(MailboxAddress.Parse(emailMessage.To));

                emailMessage.Cc?.ToList().ForEach(ccEmail => { email.Cc.Add(MailboxAddress.Parse(ccEmail)); });
                emailMessage.Bcc?.ToList().ForEach(bccEmail => { email.Bcc.Add(MailboxAddress.Parse(bccEmail)); });

                using var smtp = new SmtpClient();
                smtp.CheckCertificateRevocation = false;
                await ConnectToSmtpAsync(smtp, _provider.Settings.Smtp.Host, (int) _provider.Settings.Smtp.Port, MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable);
                
                if (_provider.Settings.Smtp.Authenticate ?? false)
                    smtp.Authenticate(_provider.Settings.Smtp.FromEmail, _provider.Settings.Smtp.Password);

                await smtp.SendAsync(email);
                smtp.Disconnect(true);

                return NotificationResult.Ok(
                    code: (int) ResultCode.OK,
                    message: "Email sent successfully using SMTP", 
                    from: _provider.Settings.Smtp.FromEmail, 
                    savesAttachments: _provider.SavesAttachments);
            }
            catch (Exception e)
            {
                return NotificationResult.Fail(
                    code: (int) ResultCode.EmailNotSent, 
                    message: $"Something went wrong when trying to send email: {e.Message}");
            }
        }

        private async Task ConnectToSmtpAsync(SmtpClient smtpClient, string host, int port, MailKit.Security.SecureSocketOptions options)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(_timeout);

            try
            {
                await smtpClient.ConnectAsync(host, port, options, cancellationTokenSource.Token);
                Console.WriteLine("Connected to SMTP server");
            }
            catch (OperationCanceledException)
            {
                
                throw new OperationCanceledException($"Could not connect to SMTP server within the estabilished time {_timeout.Seconds} seconds");
            }
        }

        private void ThrowIfSettingsNotValid()
        {
            if (string.IsNullOrWhiteSpace(_provider.Settings.Smtp.FromEmail))
                throw new ArgumentNullException(nameof(_provider.Settings.Smtp.FromEmail));

            if (string.IsNullOrWhiteSpace(_provider.Settings.Smtp.Host))
                throw new ArgumentNullException(nameof(_provider.Settings.Smtp.Host));

            if (!_provider.Settings.Smtp.Port.HasValue)
                throw new ArgumentNullException(nameof(_provider.Settings.Smtp.Port));

            if (string.IsNullOrWhiteSpace(_provider.Settings.Smtp.Password))
                throw new ArgumentNullException(nameof(_provider.Settings.Smtp.Password));
        }
    }
}