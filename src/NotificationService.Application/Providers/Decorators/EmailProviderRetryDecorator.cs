using System;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;
using Polly.Timeout;
using Polly.Wrap;
using NotificationService.Application.Providers.Interfaces;
using NotificationService.Domain.Enums;
using NotificationService.Domain.Models;
using NotificationService.Application.Common;

namespace NotificationService.Application.Providers.Decorators
{
    public partial class EmailProviderRetryDecorator : IEmailProvider
    {
        private const int MaxRetryAttempts = 5;
        private readonly TimeSpan _timeout = TimeSpan.FromSeconds(30);
        private int _triesCount = 0;
        
        
        public async Task<NotificationResult> SendAsync(EmailMessage emailMessage)
        {
            var retryPolicy = GetRetryPolicy();
            var timeoutPolicy = Policy.TimeoutAsync(_timeout, TimeoutStrategy.Pessimistic);

            var policyWrap = Policy.WrapAsync(retryPolicy, timeoutPolicy);
            return await Execute(policyWrap, emailMessage);
        }

        private AsyncRetryPolicy GetRetryPolicy()
            => Policy
                .Handle<NotificationFailedException>()
                .WaitAndRetryAsync
                (
                    MaxRetryAttempts,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        Console.WriteLine($"Attempt [{retryCount}], Retrying in {timeSpan.TotalSeconds} seconds.");
                    }
                );

        private async Task<NotificationResult> Execute(AsyncPolicyWrap policy, EmailMessage emailMessage)
        {
            var notificationResult = new NotificationResult();

            try
            {
                await policy.ExecuteAsync(async (ctx) =>
                {
                    notificationResult = await _emailProvider.SendAsync(emailMessage);
                    _triesCount++;

                    if (!notificationResult.IsSuccess && _triesCount < MaxRetryAttempts)
                        throw new NotificationFailedException();
                }, CancellationToken.None);
            }
            catch (Exception ex) when (ex is not NotificationFailedException)
            {
                bool hasPreviousMessage = !string.IsNullOrWhiteSpace(notificationResult.Message) && notificationResult.Message != ex.Message;
                
                string previousMessage = hasPreviousMessage
                    ? $": {notificationResult.Message}"
                    : string.Empty;

                string errorMessage = (ex is TimeoutRejectedException
                    ? "Timeout occurred"
                    : "An unexpected error ocurred") 
                    + previousMessage
                    + $": {ex.Message}";

                notificationResult = NotificationResult.Fail((int)ResultCode.EmailNotSent, errorMessage);
            }

            notificationResult.TriesCount = _triesCount;
            return notificationResult;
        }
    }

    public class NotificationFailedException : Exception
    {
        public NotificationFailedException() : base() {}
        public NotificationFailedException(string message) : base(message) {}
    }
}