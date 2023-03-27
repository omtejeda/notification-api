using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using NotificationService.Entities;
using NotificationService.Dtos;
using NotificationService.Dtos.Requests;
using NotificationService.Repositories;
using NotificationService.Enums;
using NotificationService.Exceptions;
using NotificationService.Services.Interfaces;
using LinqKit;
namespace NotificationService.Services
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

        public async Task<FinalResponseDTO<ProviderDTO>> CreateProvider(CreateProviderRequestDto request, string owner)
        {
            Enum.TryParse(request.Type, out ProviderType providerType);

            if (providerType == ProviderType.None)
                throw new RuleValidationException($"Provider type [{request.Type}] not valid");

            var existingProvider = await _providerRepository.FindOneAsync(x => x.Name == request.Name);
            if (existingProvider != null)
                throw new RuleValidationException($"Provider with name [{request.Name}] already exists. Created by {existingProvider.CreatedBy}");
            
            if (providerType == ProviderType.SMTP)
            {
                if (string.IsNullOrWhiteSpace(request.Settings?.Smtp.Host)) throw new RuleValidationException($"SMTP: value for {nameof(request.Settings.Smtp.Host)} is required.");
                if (!(request.Settings?.Smtp.Port).HasValue) throw new RuleValidationException($"SMTP: value for {nameof(request.Settings.Smtp.Port)} is required.");
                if (string.IsNullOrWhiteSpace(request.Settings?.Smtp.Password)) throw new RuleValidationException($"SMTP: value for {nameof(request.Settings.Smtp.Password)} is required.");
            }

            if ( (request.Settings?.Smtp != null && providerType != ProviderType.SMTP) 
              || (request.Settings?.SendGrid != null && providerType != ProviderType.SendGrid)
              || (request.Settings?.HttpClient != null && providerType != ProviderType.HttpClient))
                throw new RuleValidationException($"Provider specified {request.Type} - no need to provide settings for another provider type!");

            if (providerType == ProviderType.SendGrid)
            {
                if (string.IsNullOrWhiteSpace(request.Settings?.SendGrid.ApiKey)) throw new RuleValidationException($"SMTP: value for {nameof(request.Settings.SendGrid.ApiKey)} is required.");
            }

            if (providerType == ProviderType.HttpClient)
            {
                NotificationService.Utils.HttpUtil.CheckHTTPClientSettings(request.Settings?.HttpClient);
            }

            var provider = _mapper.Map<Provider>(request);
            provider.ProviderId = Guid.NewGuid().ToString();
            provider.CreatedBy = owner;
            provider.Type = providerType;

            var entity = await _providerRepository.InsertOneAsync(provider);
            var providerDTO = _mapper.Map<ProviderDTO>(entity);
            return new FinalResponseDTO<ProviderDTO>((int) ErrorCode.OK, providerDTO);
        }

        public async Task<FinalResponseDTO<IEnumerable<ProviderDTO>>> GetProviders(Expression<Func<Provider, bool>> filter, string owner, int? page, int? pageSize)
        {
            var filterByOwner = PredicateBuilder.New<Provider>().And(x => (x.CreatedBy == owner || x.IsPublic == true)).Expand();
            filter = filter.And(filterByOwner);

            var (providers, pagination) = await _providerRepository.FindAsync(filter, page, pageSize);
            var providersDTO = _mapper.Map<IEnumerable<ProviderDTO>>(providers);
            var paginationDTO = _mapper.Map<PaginationDTO>(pagination);

            return new FinalResponseDTO<IEnumerable<ProviderDTO>>( (int) ErrorCode.OK, providersDTO, paginationDTO);
        }

        public async Task<FinalResponseDTO<ProviderDTO>> GetProviderById(string providerId, string owner)
        {
            var provider = await _providerRepository.FindOneAsync(x => x.ProviderId == providerId);

            if (provider == null) return default;
            
            if (provider.CreatedBy != owner && !(provider.IsPublic ?? false))
                throw new RuleValidationException($"Provider was not created by {owner} and is not public");

            var providerDTO = _mapper.Map<ProviderDTO>(provider);

            return new FinalResponseDTO<ProviderDTO>((int) ErrorCode.OK, providerDTO);
        }

        public async Task DeleteProvider(string providerId, string owner)
        {
            var existingProvider = await _providerRepository.FindOneAsync(x => x.ProviderId == providerId);

            if (existingProvider == null)
                throw new RuleValidationException($"Does not exist a provider with ID [{providerId}]");

            if (existingProvider.CreatedBy != owner)
                throw new RuleValidationException($"Provider was not created by {owner}");

            await _providerRepository.DeleteOneAsync(x => x.ProviderId == providerId);
        }
    }
}