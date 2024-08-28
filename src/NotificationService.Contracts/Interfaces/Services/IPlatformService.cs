using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using NotificationService.Common.Entities;
using NotificationService.Common.Dtos;
using NotificationService.Contracts.ResponseDtos;
namespace NotificationService.Contracts.Interfaces.Services
{
    public interface IPlatformService
    {
        Task<FinalResponseDto<PlatformDto>> CreatePlatform(string name, string description, string owner);
        Task DeletePlatform(string platformId, string owner);
        Task<FinalResponseDto<IEnumerable<PlatformDto>>> GetPlatforms(Expression<Func<Platform, bool>> filter, string owner, int? page, int? pageSize);
        Task<FinalResponseDto<PlatformDto>> GetPlatformById(string platformId, string owner);
    }
}