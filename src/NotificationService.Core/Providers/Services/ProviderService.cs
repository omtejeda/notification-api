using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using NotificationService.Core.Common.Exceptions;
using LinqKit;
using NotificationService.Common.Entities;
using NotificationService.Common.Enums;
using NotificationService.Common.Dtos;
using NotificationService.Contracts.RequestDtos;
using NotificationService.Contracts.Interfaces.Services;
using NotificationService.Contracts.Interfaces.Repositories;
using NotificationService.Contracts.ResponseDtos;
using System.Linq;
using NotificationService.Common.Resources;
namespace NotificationService.Core.Providers.Services
{
    public class ProviderService : IProviderService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Provider> _providerRepository;

        public ProviderService(IRepository<Provider> providerRepository, IMapper mapper)
        {
            _providerRepository = providerRepository;
            _mapper = mapper;
        }

        public async Task<FinalResponseDto<ProviderDto>> CreateProvider(CreateProviderRequestDto request, string owner)
        {
            Enum.TryParse(request.Type, out ProviderType providerType);

            if (providerType == ProviderType.None)
                throw new RuleValidationException(string.Format(Messages.ProviderTypeNotValid, request.Type));

            var existingProvider = await _providerRepository.FindOneAsync(x => x.Name.ToLower() == request.Name.ToLower());
            if (existingProvider is not null)
                throw new RuleValidationException(string.Format(Messages.ProviderAlreadyExists, request.Name, existingProvider.CreatedBy));
            
            if (providerType == ProviderType.SMTP)
            {
                if (string.IsNullOrWhiteSpace(request.Settings?.Smtp.Host)) throw new RuleValidationException(string.Format(Messages.RequiredValue, nameof(request.Settings.Smtp.Host)));
                if (!(request.Settings?.Smtp.Port).HasValue) throw new RuleValidationException(string.Format(Messages.RequiredValue, nameof(request.Settings.Smtp.Port)));
                if (string.IsNullOrWhiteSpace(request.Settings?.Smtp.Password)) throw new RuleValidationException(string.Format(Messages.RequiredValue, nameof(request.Settings.Smtp.Password)));
            }

            if ( (request.Settings?.Smtp is not null && providerType != ProviderType.SMTP) 
              || (request.Settings?.SendGrid is not null && providerType != ProviderType.SendGrid)
              || (request.Settings?.HttpClient is not null && providerType != ProviderType.HttpClient))
                throw new RuleValidationException(string.Format(Messages.ProviderSettingsConflict, request.Type));

            if (providerType == ProviderType.SendGrid)
            {
                if (string.IsNullOrWhiteSpace(request.Settings?.SendGrid.ApiKey)) throw new RuleValidationException(string.Format(Messages.RequiredValue, nameof(request.Settings.SendGrid.ApiKey)));
            }

            if (providerType == ProviderType.HttpClient)
            {
                Common.Utils.HttpUtil.CheckHTTPClientSettings(request.Settings?.HttpClient);
            }

            var provider = _mapper.Map<Provider>(request);
            provider.ProviderId = Guid.NewGuid().ToString();
            provider.CreatedBy = owner;
            provider.Type = providerType;

            var entity = await _providerRepository.InsertOneAsync(provider);
            var providerDTO = _mapper.Map<ProviderDto>(entity);
            return new FinalResponseDto<ProviderDto>((int) ErrorCode.OK, providerDTO);
        }

        public async Task<FinalResponseDto<IEnumerable<ProviderDto>>> GetProviders(Expression<Func<Provider, bool>> filter, string owner, int? page, int? pageSize)
        {
            var filterByOwner = PredicateBuilder.New<Provider>().And(x => (x.CreatedBy == owner || x.IsPublic == true)).Expand();
            filter = filter.And(filterByOwner);

            var (providers, pagination) = await _providerRepository.FindAsync(filter, page, pageSize);
            var providersDTO = _mapper.Map<IEnumerable<ProviderDto>>(providers);
            var paginationDTO = _mapper.Map<PaginationDto>(pagination);

            return new FinalResponseDto<IEnumerable<ProviderDto>>( (int) ErrorCode.OK, providersDTO, paginationDTO);
        }

        public async Task<FinalResponseDto<ProviderDto>> GetProviderById(string providerId, string owner)
        {
            var provider = await _providerRepository.FindOneAsync(x => x.ProviderId == providerId);

            if (provider is null) return default;
            
            if (provider.CreatedBy != owner && !(provider.IsPublic ?? false))
                throw new RuleValidationException(string.Format(Messages.ProviderIsNotPublicNeitherWasCreatedByYou, owner));

            var providerDTO = _mapper.Map<ProviderDto>(provider);

            return new FinalResponseDto<ProviderDto>((int) ErrorCode.OK, providerDTO);
        }

        public async Task DeleteProvider(string providerId, string owner)
        {
            var existingProvider = await _providerRepository.FindOneAsync(x => x.ProviderId == providerId);

            if (existingProvider is null)
                throw new RuleValidationException(string.Format(Messages.ProviderWithGivenIdNotExists, providerId));

            if (existingProvider.CreatedBy != owner)
                throw new RuleValidationException(string.Format(Messages.ProviderWasNotCreatedByYou, owner));

            await _providerRepository.DeleteOneAsync(x => x.ProviderId == providerId);
        }

        public async Task AddToWhiteList(string providerId, string owner, string recipient)
        {
            var provider = await _providerRepository.FindOneAsync(x => x.ProviderId == providerId);
            
            if (provider is null)
                throw new RuleValidationException(string.Format(Messages.ProviderWithGivenIdNotExists, providerId));

            if (provider.CreatedBy != owner && !(provider.IsPublic ?? false))
                throw new RuleValidationException(string.Format(Messages.ProviderIsNotPublicNeitherWasCreatedByYou, owner));
            
            provider.DevSettings ??= new();
            provider.DevSettings.AllowedRecipients ??= new List<string>();
            if (provider.DevSettings.AllowedRecipients.Any(x => x.ToLower() == recipient.ToLower()))
            {
                throw new RuleValidationException(Messages.RecipientAlreadyExists);
            }

            provider.DevSettings.AllowedRecipients.Add(recipient.ToLower());
            
            await _providerRepository.UpdateOneByIdAsync(provider.Id, provider);
        }

        public async Task DeleteFromWhiteList(string providerId, string owner, string recipient)
        {
            var provider = await _providerRepository.FindOneAsync(x => x.ProviderId == providerId);
            
            if (provider is null)
                throw new RuleValidationException(string.Format(Messages.ProviderWithGivenIdNotExists, providerId));

            if (provider.CreatedBy != owner && !(provider.IsPublic ?? false))
                throw new RuleValidationException(string.Format(Messages.ProviderIsNotPublicNeitherWasCreatedByYou, owner));
            
            var existingRecipient = provider?.DevSettings?.AllowedRecipients.FirstOrDefault(x => x.ToLower() == recipient.ToLower());

            if (existingRecipient is null)
                throw new RuleValidationException(string.Format(Messages.RecipientNotExists, recipient));

            provider.DevSettings.AllowedRecipients.Remove(existingRecipient);
            
            await _providerRepository.UpdateOneByIdAsync(provider.Id, provider);
        }
    }
}