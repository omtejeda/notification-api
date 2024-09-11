using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using NotificationService.Domain.Entities;
using NotificationService.Common.Dtos;
using NotificationService.Contracts.RequestDtos;
using NotificationService.Contracts.ResponseDtos;

namespace NotificationService.Contracts.Interfaces.Services
{
    public interface IProviderService
    {
        Task<BaseResponse<ProviderDto>> CreateProvider(CreateProviderRequestDto request, string owner);
        Task<BaseResponse<IEnumerable<ProviderDto>>> GetProviders(Expression<Func<Provider, bool>> filter, string owner, int? page, int? pageSize);
        Task<BaseResponse<ProviderDto>> GetProviderById(string providerId, string owner);
        Task DeleteProvider(string providerId, string owner);
        Task AddToWhiteList(string providerId, string owner, string destination);
        Task DeleteFromWhiteList(string providerId, string owner, string destination);
    }
}