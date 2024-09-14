using System.Linq.Expressions;
using AutoMapper;
using NotificationService.Application.Exceptions;
using LinqKit;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Enums;
using NotificationService.Common.Dtos;
using NotificationService.Application.Contracts.RequestDtos;
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Application.Contracts.Interfaces.Repositories;
using NotificationService.Common.Resources;
using NotificationService.Application.Utils;
namespace NotificationService.Application.Providers.Services
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

        public async Task<BaseResponse<ProviderDto>> CreateProvider(CreateProviderRequestDto request, string owner)
        {
            Enum.TryParse(request.Type, out ProviderType providerType);

            Guard.ProviderTypeIsValid(providerType);
            
            var existingProvider = await _providerRepository.FindOneAsync(x => x.Name.ToLower() == request.Name.ToLower());
            Guard.ProviderNotExists(existingProvider);
            
            if (providerType == ProviderType.SMTP)
            {
                Guard.RequiredValueIsPresent(request.Settings?.Smtp.Host, nameof(request.Settings.Smtp.Host));
                Guard.RequiredValueIsPresent(request.Settings?.Smtp.Port, nameof(request.Settings.Smtp.Port));
                Guard.RequiredValueIsPresent(request.Settings?.Smtp.Password, nameof(request.Settings.Smtp.Password));
            }

            if ( (request.Settings?.Smtp is not null && providerType != ProviderType.SMTP) 
              || (request.Settings?.SendGrid is not null && providerType != ProviderType.SendGrid)
              || (request.Settings?.HttpClient is not null && providerType != ProviderType.HttpClient))
                throw new RuleValidationException(string.Format(Messages.ProviderSettingsConflict, request.Type));

            if (providerType == ProviderType.SendGrid)
            {
                Guard.RequiredValueIsPresent(request.Settings?.SendGrid.ApiKey, nameof(request.Settings.SendGrid.ApiKey));
            }

            if (providerType == ProviderType.HttpClient)
            {
                Utils.HttpUtil.CheckHTTPClientSettings(request.Settings?.HttpClient);
            }

            var provider = _mapper.Map<Provider>(request);
            provider.ProviderId = Guid.NewGuid().ToString();
            provider.CreatedBy = owner;
            provider.Type = providerType;

            var entity = await _providerRepository.InsertOneAsync(provider);
            var providerDto = _mapper.Map<ProviderDto>(entity);
            
            return BaseResponse<ProviderDto>.Success(providerDto);
        }

        public async Task<BaseResponse<IEnumerable<ProviderDto>>> GetProviders(Expression<Func<Provider, bool>> filter, string owner, int? page, int? pageSize)
        {
            var filterByOwner = PredicateBuilder.New<Provider>().And(x => (x.CreatedBy == owner || x.IsPublic == true)).Expand();
            filter = filter.And(filterByOwner);

            var (providers, pagination) = await _providerRepository.FindAsync(filter, page, pageSize);
            var providersDto = _mapper.Map<IEnumerable<ProviderDto>>(providers);
            var paginationDto = _mapper.Map<PaginationDto>(pagination);

            return BaseResponse<IEnumerable<ProviderDto>>.Success(providersDto, paginationDto);
        }

        public async Task<BaseResponse<ProviderDto>> GetProviderById(string providerId, string owner)
        {
            var provider = await _providerRepository.FindOneAsync(x => x.ProviderId == providerId);

            if (provider is null) return default!;

            Guard.ProviderIsCreatedByRequesterOrPublic(provider, owner);
            var providerDto = _mapper.Map<ProviderDto>(provider);

            return BaseResponse<ProviderDto>.Success(providerDto);
        }

        public async Task DeleteProvider(string providerId, string owner)
        {
            var existingProvider = await _providerRepository.FindOneAsync(x => x.ProviderId == providerId);

            Guard.ProviderWithIdExists(existingProvider, providerId);
            Guard.ProviderIsCreatedByRequester(existingProvider.CreatedBy, owner);

            await _providerRepository.DeleteOneAsync(x => x.ProviderId == providerId);
        }

        public async Task AddToWhiteList(string providerId, string owner, string recipient)
        {
            var provider = await _providerRepository.FindOneAsync(x => x.ProviderId == providerId);
            
            Guard.ProviderWithIdExists(provider, providerId);
            Guard.ProviderIsCreatedByRequesterOrPublic(provider, owner);
            
            provider.DevSettings ??= new();
            provider.DevSettings.AllowedRecipients ??= new List<string>();

            Guard.RecipientNotExists(provider, recipient);
            provider.DevSettings.AllowedRecipients.Add(recipient.ToLower());
            
            await _providerRepository.UpdateOneByIdAsync(provider.Id, provider);
        }

        public async Task DeleteFromWhiteList(string providerId, string owner, string recipient)
        {
            var provider = await _providerRepository.FindOneAsync(x => x.ProviderId == providerId);
            
            Guard.ProviderWithIdExists(provider, providerId);
            Guard.ProviderIsCreatedByRequesterOrPublic(provider, owner);
            
            var existingRecipient = provider?.DevSettings?.AllowedRecipients.FirstOrDefault(x => x.ToLower() == recipient.ToLower());

            Guard.RecipientExists(existingRecipient);
            provider!.DevSettings.AllowedRecipients.Remove(existingRecipient!);
            
            await _providerRepository.UpdateOneByIdAsync(provider.Id, provider);
        }
    }
}