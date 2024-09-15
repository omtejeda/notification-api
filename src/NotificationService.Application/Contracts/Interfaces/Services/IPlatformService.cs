using System.Linq.Expressions;
using NotificationService.Domain.Entities;
using NotificationService.Common.Dtos;

namespace NotificationService.Application.Contracts.Interfaces.Services
{
    public interface IPlatformService
    {
        Task<BaseResponse<PlatformDto>> CreatePlatform(string name, string description, string owner);
        Task DeletePlatform(string platformId, string owner);
        Task<BaseResponse<IEnumerable<PlatformDto>>> GetPlatforms(Expression<Func<Platform, bool>> filter, string owner, int? page, int? pageSize);
        Task<BaseResponse<PlatformDto>> GetPlatformById(string platformId, string owner);
    }
}