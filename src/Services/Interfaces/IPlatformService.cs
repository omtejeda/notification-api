using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using NotificationService.Entities;
using NotificationService.Dtos;
namespace NotificationService.Services.Interfaces
{
    public interface IPlatformService
    {
        Task<FinalResponseDTO<PlatformDTO>> CreatePlatform(string name, string description, string owner);
        Task DeletePlatform(string platformId, string owner);
        Task<FinalResponseDTO<IEnumerable<PlatformDTO>>> GetPlatforms(Expression<Func<Platform, bool>> filter, string owner, int? page, int? pageSize);
        Task<FinalResponseDTO<PlatformDTO>> GetPlatformById(string platformId, string owner);
    }
}