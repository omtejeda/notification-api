using System.Linq.Expressions;
using NotificationService.Domain.Entities;
using NotificationService.Application.Common.Dtos;
using NotificationService.Application.Common.Models;

namespace NotificationService.Application.Contracts.Interfaces.Services;

/// <summary>
/// Defines the contract for managing platforms, including creation, deletion, and retrieval of platform data.
/// </summary>
public interface IPlatformService
{
    /// <summary>
    /// Creates a new platform.
    /// </summary>
    /// <param name="name">The name of the platform.</param>
    /// <param name="description">A description of the platform.</param>
    /// <param name="owner">The owner of the platform.</param>
    /// <returns>A <see cref="BaseResponse{PlatformDto}"/> containing the created platform's details.</returns>
    Task<BaseResponse<PlatformDto>> CreatePlatform(string name, string description, string owner);

    /// <summary>
    /// Deletes a platform by its unique identifier.
    /// </summary>
    /// <param name="platformId">The unique identifier of the platform to delete.</param>
    /// <param name="owner">The owner of the platform.</param>
    Task DeletePlatform(string platformId, string owner);

    /// <summary>
    /// Retrieves a list of platforms that match the specified filter criteria.
    /// </summary>
    /// <param name="filter">The filter expression to apply to the platforms.</param>
    /// <param name="owner">The owner of the platforms.</param>
    /// <param name="filterOptions">Additional filtering and pagination options.</param>
    /// <returns>A <see cref="BaseResponse{IEnumerable{PlatformDto}}"/> containing the matching platforms.</returns>
    Task<BaseResponse<IEnumerable<PlatformDto>>> GetPlatforms(Expression<Func<Platform, bool>> filter, string owner, FilterOptions filterOptions);

    /// <summary>
    /// Retrieves a platform by its unique identifier.
    /// </summary>
    /// <param name="platformId">The unique identifier of the platform to retrieve.</param>
    /// <param name="owner">The owner of the platform.</param>
    /// <returns>A <see cref="BaseResponse{PlatformDto}"/> containing the platform's details.</returns>
    Task<BaseResponse<PlatformDto>> GetPlatformById(string platformId, string owner);
}