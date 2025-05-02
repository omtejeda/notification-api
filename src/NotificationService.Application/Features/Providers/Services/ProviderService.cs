using System.Linq.Expressions;
using AutoMapper;
using NotificationService.Application.Exceptions;
using LinqKit;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Enums;
using NotificationService.Application.Contracts.DTOs.Requests;
using NotificationService.Application.Contracts.Services;
using NotificationService.Application.Contracts.Persistence;
using NotificationService.SharedKernel.Resources;
using NotificationService.Application.Common.Helpers;
using NotificationService.Application.Common.Dtos;
using NotificationService.Application.Common.Models;

namespace NotificationService.Application.Features.Providers.Services;

public class ProviderService(IRepository<Provider> providerRepository, IMapper mapper) : IProviderService
{
    private readonly IMapper _mapper = mapper;
    private readonly IRepository<Provider> _providerRepository = providerRepository;

    public async Task<BaseResponse<ProviderDto>> CreateProvider(CreateProviderRequestDto request, string owner)
    {
        _ = Enum.TryParse(request.Type, out ProviderType providerType);

        Guard.ProviderTypeIsValid(providerType);
        
        var existingProvider = await _providerRepository.FindOneAsync(x => x.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase));
        Guard.ProviderNotExists(existingProvider);

        if ((request.Settings?.Smtp is not null && providerType != ProviderType.SMTP) 
            || (request.Settings?.SendGrid is not null && providerType != ProviderType.SendGrid)
            || (request.Settings?.HttpClient is not null && providerType != ProviderType.HttpClient)
            || (request.Settings?.Firebase is not null && providerType != ProviderType.Firebase))
        {
            throw new RuleValidationException(string.Format(Messages.ProviderSettingsConflict, request.Type));
        }

        switch (providerType)
        {
            case ProviderType.SendGrid:
                CheckSendGridSettings(request.Settings?.SendGrid);
                break;
            case ProviderType.HttpClient:
                HttpRequestHelper.CheckHTTPClientSettings(request.Settings?.HttpClient);
                break;
            case ProviderType.Firebase:
                CheckFirebaseSettings(request.Settings?.Firebase);
                break;
            case ProviderType.SMTP:
                CheckSmtpSettings(request.Settings?.Smtp);
                break;
        };

        var provider = _mapper.Map<Provider>(request);
        provider.ProviderId = Guid.NewGuid().ToString();
        provider.CreatedBy = owner;
        provider.Type = providerType;

        var entity = await _providerRepository.InsertOneAsync(provider);
        var providerDto = _mapper.Map<ProviderDto>(entity);
        
        return BaseResponse<ProviderDto>.Success(providerDto);
    }

    public async Task<BaseResponse<IEnumerable<ProviderDto>>> GetProviders(Expression<Func<Provider, bool>> filter, string owner, FilterOptions filterOptions)
    {
        var filterByOwner = PredicateBuilder.New<Provider>().And(x => (x.CreatedBy == owner || x.IsPublic == true)).Expand();
        filter = filter.And(filterByOwner);

        var (providers, pagination) = await _providerRepository.FindAsync(filter, filterOptions);
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
        Guard.ProviderIsCreatedByRequester(existingProvider?.CreatedBy, owner);

        await _providerRepository.DeleteOneAsync(x => x.ProviderId == providerId);
    }

    public async Task AddToWhiteList(string providerId, string owner, string recipient)
    {
        var provider = await _providerRepository.FindOneAsync(x => x.ProviderId == providerId);
        
        Guard.ProviderWithIdExists(provider, providerId);
        Guard.ProviderIsCreatedByRequesterOrPublic(provider, owner);
        
        provider.DevSettings ??= new();
        provider.DevSettings.AllowedRecipients ??= [];

        Guard.RecipientNotExists(provider, recipient);
        provider.DevSettings.AllowedRecipients.Add(recipient.ToLower());
        
        await _providerRepository.UpdateOneByIdAsync(provider.Id, provider);
    }

    public async Task DeleteFromWhiteList(string providerId, string owner, string recipient)
    {
        var provider = await _providerRepository.FindOneAsync(x => x.ProviderId == providerId);
        
        Guard.ProviderWithIdExists(provider, providerId);
        Guard.ProviderIsCreatedByRequesterOrPublic(provider, owner);
        
        var existingRecipient = provider?.DevSettings?.AllowedRecipients.FirstOrDefault(x => x.Equals(recipient, StringComparison.OrdinalIgnoreCase));

        Guard.RecipientExists(existingRecipient);
        provider!.DevSettings.AllowedRecipients.Remove(existingRecipient);
        
        await _providerRepository.UpdateOneByIdAsync(provider.Id, provider);
    }

    private static void CheckFirebaseSettings(FirebaseSettingDto? firebaseSetting)
    {
        Guard.RequiredValueIsPresent(firebaseSetting?.Type, nameof(firebaseSetting.Type));
        Guard.RequiredValueIsPresent(firebaseSetting?.ProjectId, nameof(firebaseSetting.ProjectId));
        Guard.RequiredValueIsPresent(firebaseSetting?.PrivateKeyId, nameof(firebaseSetting.PrivateKeyId));
        Guard.RequiredValueIsPresent(firebaseSetting?.PrivateKey, nameof(firebaseSetting.PrivateKey));
        Guard.RequiredValueIsPresent(firebaseSetting?.ClientEmail, nameof(firebaseSetting.ClientEmail));
        Guard.RequiredValueIsPresent(firebaseSetting?.ClientId, nameof(firebaseSetting.ClientId));
        Guard.RequiredValueIsPresent(firebaseSetting?.AuthUri, nameof(firebaseSetting.AuthUri));
        Guard.RequiredValueIsPresent(firebaseSetting?.TokenUri, nameof(firebaseSetting.TokenUri));
        Guard.RequiredValueIsPresent(firebaseSetting?.AuthProviderX509CertUrl, nameof(firebaseSetting.AuthProviderX509CertUrl));
        Guard.RequiredValueIsPresent(firebaseSetting?.ClientX509CertUrl, nameof(firebaseSetting.ClientX509CertUrl));
    }

    private static void CheckSmtpSettings(SmtpSettingDto? smtpSetting)
    {
        Guard.RequiredValueIsPresent(smtpSetting?.Host, nameof(smtpSetting.Host));
        Guard.RequiredValueIsPresent(smtpSetting?.Port, nameof(smtpSetting.Port));
        Guard.RequiredValueIsPresent(smtpSetting?.Password, nameof(smtpSetting.Password));
    }

    private static void CheckSendGridSettings(SendGridSettingDto? setting)
    {
        Guard.RequiredValueIsPresent(setting?.ApiKey, nameof(setting.ApiKey));
    }
}