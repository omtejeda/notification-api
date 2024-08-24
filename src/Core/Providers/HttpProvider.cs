using System;
using System.Threading.Tasks;
using NotificationService.Core.Common;
using NotificationService.Core.Providers.Entities;

namespace NotificationService.Core.Providers
{
    public class HttpProvider
    {
        private readonly Provider _provider;

        public HttpProvider(Provider provider)
        {
            _provider = provider;
        }
        
        public async Task<NotificationResult> SendAsync(EmailMessage emailMessage)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}