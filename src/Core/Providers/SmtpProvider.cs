using System;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using NotificationService.Core.Common.Enums;
using NotificationService.Core.Common.Utils;
using NotificationService.Core.Common;
using NotificationService.Core.Providers.Enums;
using NotificationService.Core.Providers.Entities;
using NotificationService.Core.Providers.Interfaces;
using System.Threading;

namespace NotificationService.Core.Providers
{
    public class SmtpProvider : IEmailProvider
    {
        public ProviderType ProviderType => ProviderType.SMTP;
        private Provider _provider;
        private readonly TimeSpan _timeout = TimeSpan.FromSeconds(10);

        public void SetProvider(Provider provider)
        {
            _provider = provider;
        }
        
        public async Task<NotificationResult> SendAsync(EmailMessage emailMessage)
        {
            EmailUtil.ThrowIfEmailNotAllowed(provider: _provider, to: emailMessage.To, cc: emailMessage.Cc, bcc: emailMessage.Bcc);
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
                    code: (int) ErrorCode.OK,
                    message: "Email sent successfully using SMTP", 
                    from: _provider.Settings.Smtp.FromEmail, 
                    savesAttachments: _provider.SavesAttachments);
            }
            catch (Exception e)
            {
                return NotificationResult.Fail(
                    code: (int) ErrorCode.EmailNotSent, 
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
                Console.WriteLine("Conectado a server SMTP");
            }
            catch (OperationCanceledException)
            {
                throw new OperationCanceledException($"No se logró conectar al servidor SMTP dentro del tiempo establecido: {_timeout.Seconds} segundos");
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