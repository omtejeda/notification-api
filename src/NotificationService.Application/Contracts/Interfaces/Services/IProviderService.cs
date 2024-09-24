using System.Linq.Expressions;
using NotificationService.Domain.Entities;
using NotificationService.Application.Contracts.RequestDtos;
using NotificationService.Application.Common.Dtos;
using NotificationService.Application.Common.Models;

namespace NotificationService.Application.Contracts.Interfaces.Services;

/// <summary>
/// Defines the contract for managing providers, including creation, retrieval, and management of whitelist entries.
/// </summary>
public interface IProviderService
{
    /// <summary>
    /// Creates a new provider.
    /// </summary>
    /// <param name="request">The request DTO containing the provider's details.</param>
    /// <param name="owner">The owner of the provider.</param>
    /// <returns>A <see cref="BaseResponse{ProviderDto}"/> containing the created provider's details.</returns>
    Task<BaseResponse<ProviderDto>> CreateProvider(CreateProviderRequestDto request, string owner);

    /// <summary>
    /// Retrieves a list of providers that match the specified filter criteria.
    /// </summary>
    /// <param name="filter">The filter expression to apply to the providers.</param>
    /// <param name="owner">The owner of the providers.</param>
    /// <param name="filterOptions">Additional filtering and pagination options.</param>
    /// <returns>A <see cref="BaseResponse{IEnumerable{ProviderDto}}"/> containing the matching providers.</returns>
    Task<BaseResponse<IEnumerable<ProviderDto>>> GetProviders(Expression<Func<Provider, bool>> filter, string owner, FilterOptions filterOptions);

    /// <summary>
    /// Retrieves a provider by its unique identifier.
    /// </summary>
    /// <param name="providerId">The unique identifier of the provider to retrieve.</param>
    /// <param name="owner">The owner of the provider.</param>
    /// <returns>A <see cref="BaseResponse{ProviderDto}"/> containing the provider's details.</returns>
    Task<BaseResponse<ProviderDto>> GetProviderById(string providerId, string owner);

    /// <summary>
    /// Deletes a provider by its unique identifier.
    /// </summary>
    /// <param name="providerId">The unique identifier of the provider to delete.</param>
    /// <param name="owner">The owner of the provider.</param>
    Task DeleteProvider(string providerId, string owner);

    /// <summary>
    /// Adds a recipient to the whitelist for a specific provider.
    /// </summary>
    /// <param name="providerId">The unique identifier of the target provider.</param>
    /// <param name="owner">The owner of the provider.</param>
    /// <param name="destination">The recipient to be added to the whitelist.</param>
    Task AddToWhiteList(string providerId, string owner, string destination);

    /// <summary>
    /// Deletes a recipient from the whitelist for a specific provider.
    /// </summary>
    /// <param name="providerId">The unique identifier of the provider to remove from the whitelist.</param>
    /// <param name="owner">The owner of the provider.</param>
    /// <param name="destination">The recipient to be removed from the whitelist.</param>
    Task DeleteFromWhiteList(string providerId, string owner, string destination);
}