using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using NotificationService.Entities;
using NotificationService.Interfaces;
using NotificationService.Dtos;
using NotificationService.Dtos.Requests;

namespace NotificationService.Services.Interfaces
{
    public interface IProviderService
    {
        Task<FinalResponseDTO<ProviderDTO>> CreateProvider(CreateProviderRequestDto request, string owner);
        Task<FinalResponseDTO<IEnumerable<ProviderDTO>>> GetProviders(Expression<Func<Provider, bool>> filter, string owner, int? page, int? pageSize);
        Task<FinalResponseDTO<ProviderDTO>> GetProviderById(string providerId, string owner);
        Task DeleteProvider(string providerId, string owner);
    }
}