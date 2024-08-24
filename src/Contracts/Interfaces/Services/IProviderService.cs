using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using NotificationService.Core.Providers.Entities;
using NotificationService.Core.Providers.Dtos;
using NotificationService.Contracts.RequestDtos;
using NotificationService.Contracts.ResponseDtos;

namespace NotificationService.Contracts.Interfaces.Services
{
    public interface IProviderService
    {
        Task<FinalResponseDTO<ProviderDTO>> CreateProvider(CreateProviderRequestDto request, string owner);
        Task<FinalResponseDTO<IEnumerable<ProviderDTO>>> GetProviders(Expression<Func<Provider, bool>> filter, string owner, int? page, int? pageSize);
        Task<FinalResponseDTO<ProviderDTO>> GetProviderById(string providerId, string owner);
        Task DeleteProvider(string providerId, string owner);
        Task AddToWhiteList(string providerId, string owner, string destination);
        Task DeleteFromWhiteList(string providerId, string owner, string destination);
    }
}