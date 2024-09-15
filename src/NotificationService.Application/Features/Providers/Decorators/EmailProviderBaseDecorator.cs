using NotificationService.Domain.Entities;
using NotificationService.Domain.Enums;
using NotificationService.Application.Features.Providers.Interfaces;

namespace NotificationService.Application.Features.Providers.Decorators
{
    public partial class EmailProviderRetryDecorator : IEmailProvider
    {
        private IEmailProvider _emailProvider;
        public ProviderType ProviderType => _emailProvider.ProviderType;
        
        public EmailProviderRetryDecorator(IEmailProvider emailProvider)
            => _emailProvider = emailProvider;
        
        public void SetProvider(Provider provider)
            => _emailProvider.SetProvider(provider);
    }
}