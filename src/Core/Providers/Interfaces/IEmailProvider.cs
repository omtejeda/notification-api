using System.Threading.Tasks;
using NotificationService.Core.Common;
using NotificationService.Core.Providers.Entities;
using NotificationService.Core.Providers.Enums;

namespace NotificationService.Core.Providers.Interfaces
{
    public interface IEmailProvider
    {
        public ProviderType ProviderType { get; }
        public void SetProvider(Provider provider);
        
        Task<NotificationResult> SendAsync(EmailMessage emailMessage);   
    }
}