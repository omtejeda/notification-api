using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NotificationService.Domain.Entities;
using NotificationService.Core.Providers.Factories.Interfaces;
using NotificationService.Contracts.Interfaces.Repositories;
using NotificationService.Core.Providers.Interfaces;
using NotificationService.Common.Utils;

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
            Guard.ProviderNameAndCreatedByHasValue(providerName, createdBy);
            var provider = await FindProviderAsync(providerName, createdBy);
            var emailProvider = _emailProviders.FirstOrDefault(x => x.ProviderType == provider.Type);

            Guard.EmailProviderIsNotNull(emailProvider);
            emailProvider!.SetProvider(provider);
            
            return emailProvider;
        }

        private async Task<Provider> FindProviderAsync(string providerName, string createdBy)
        {
            var provider = await _providerRepository.FindOneAsync(x => x.Name.Equals(providerName));

            Guard.ProviderIsNotNull(provider, providerName);
            Guard.ProviderIsCreatedByRequesterOrPublic(provider, createdBy);

            return provider;
        }
    }
}