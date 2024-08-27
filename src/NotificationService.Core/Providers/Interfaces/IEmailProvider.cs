using System.Threading.Tasks;
using NotificationService.Core.Common;
using NotificationService.Common.Entities;
using NotificationService.Common.Enums;
using NotificationService.Common.Models;

namespace NotificationService.Core.Providers.Interfaces
{
    public interface IEmailProvider
    {
        public ProviderType ProviderType { get; }
        public void SetProvider(Provider provider);
        
        Task<NotificationResult> SendAsync(EmailMessage emailMessage);   
    }
}