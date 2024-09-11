using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using NotificationService.Domain.Entities;
using NotificationService.Common.Dtos;
using NotificationService.Core.Contracts.ResponseDtos;
namespace NotificationService.Core.Contracts.Interfaces.Services
{
    public interface IPlatformService
    {
        Task<BaseResponse<PlatformDto>> CreatePlatform(string name, string description, string owner);
        Task DeletePlatform(string platformId, string owner);
        Task<BaseResponse<IEnumerable<PlatformDto>>> GetPlatforms(Expression<Func<Platform, bool>> filter, string owner, int? page, int? pageSize);
        Task<BaseResponse<PlatformDto>> GetPlatformById(string platformId, string owner);
    }
}