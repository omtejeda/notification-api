using System.Threading.Tasks;
using NotificationService.Core.Common;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Enums;
using NotificationService.Domain.Models;

namespace NotificationService.Core.Providers.Interfaces
{
    public interface IEmailProvider
    {
        public ProviderType ProviderType { get; }
        public void SetProvider(Provider provider);
        
        Task<NotificationResult> SendAsync(EmailMessage emailMessage);   
    }
}