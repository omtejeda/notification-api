using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NotificationService.Common.Entities;
using NotificationService.Core.Providers.Factories.Interfaces;
using NotificationService.Core.Common.Exceptions;
using NotificationService.Contracts.Interfaces.Repositories;
using NotificationService.Core.Providers.Interfaces;
using NotificationService.Common.Resources;

namespace NotificationService.Core.Providers.Factories
{
    public class EmailProviderFactory : IEmailProviderFactory
    {
        private readonly IRepository<Provider> _providerRepository;
        private readonly IEnumerable<IEmailProvider> _emailProviders;

        public EmailProviderFactory(IRepository<Provider> providerRepository, IEnumerable<IEmailProvider> emailProviders)
        {
            _providerRepository = providerRepository;
            _emailProviders = emailProviders;
        }

        public async Task<IEmailProvider> CreateProviderAsync(string providerName, string createdBy)
        {
            ThrowIfProviderNameOrCreatedByIsNull(providerName, createdBy);
            var provider = await FindProviderAsync(providerName, createdBy);

            var emailProvider = _emailProviders.FirstOrDefault(x => x.ProviderType == provider.Type);

            if (emailProvider is null)
                throw new ArgumentException("Underlying provider could not be found");

            emailProvider.SetProvider(provider);
            
            return emailProvider;
        }

        private async Task<Provider> FindProviderAsync(string providerName, string createdBy)
        {
            var provider = await _providerRepository.FindOneAsync(x => x.Name.Equals(providerName));

            if (provider is null)
                throw new RuleValidationException(string.Format(Messages.ProviderSpecifiedNotExists, providerName));

            if (!(provider.IsPublic ?? false) && provider.CreatedBy != createdBy)
                throw new RuleValidationException(Messages.ProviderIsNotPublicNeitherWasCreatedByYou);

            return provider;
        }

        private void ThrowIfProviderNameOrCreatedByIsNull(string providerName, string createdBy)
        {
            if (string.IsNullOrWhiteSpace(providerName))
                throw new ArgumentNullException(nameof(providerName));
            
            if (string.IsNullOrWhiteSpace(createdBy))
                throw new ArgumentNullException(nameof(createdBy));
        }
    }
}