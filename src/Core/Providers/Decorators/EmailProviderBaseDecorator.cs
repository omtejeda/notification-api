using NotificationService.Core.Providers.Entities;
using NotificationService.Core.Providers.Enums;
using NotificationService.Core.Providers.Interfaces;

namespace NotificationService.Core.Providers.Decorators
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