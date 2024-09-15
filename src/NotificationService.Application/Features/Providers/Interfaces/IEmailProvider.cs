using NotificationService.Application.Features.Senders.Models;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Enums;
using NotificationService.Domain.Models;

namespace NotificationService.Application.Features.Providers.Interfaces
{
    public interface IEmailProvider
    {
        public ProviderType ProviderType { get; }
        public void SetProvider(Provider provider);
        
        Task<NotificationResult> SendAsync(EmailMessage emailMessage);   
    }
}