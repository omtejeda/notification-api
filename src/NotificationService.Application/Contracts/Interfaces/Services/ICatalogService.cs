using System.Linq.Expressions;
using NotificationService.Domain.Entities;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Application.Common.Models;

namespace NotificationService.Application.Contracts.Interfaces.Services;

/// <summary>
/// Provides methods for managing catalogs, including creation, deletion, and retrieval of catalog data.
/// </summary>
public interface ICatalogService
{
    /// <summary>
    /// Creates a new catalog with the specified details.
    /// </summary>
    /// <param name="name">The name of the catalog.</param>
    /// <param name="description">The description of the catalog.</param>
    /// <param name="isActive">Indicates whether the catalog is active.</param>
    /// <param name="elements">A collection of elements to be associated with the catalog.</param>
    /// <param name="owner">The owner of the catalog.</param>
    /// <returns>A <see cref="BaseResponse{CatalogDto}"/> containing the created catalog details.</returns>
    Task<BaseResponse<CatalogDto>> CreateCatalog(string name, string description, bool isActive, ICollection<ElementDto> elements, string owner);

    /// <summary>
    /// Deletes the catalog with the specified identifier.
    /// </summary>
    /// <param name="catalogId">The identifier of the catalog to be deleted.</param>
    /// <param name="owner">The owner of the catalog.</param>
    Task DeleteCatalog(string catalogId, string owner);

    /// <summary>
    /// Retrieves a paginated list of catalogs based on the specified filter.
    /// </summary>
    /// <param name="filter">The filter expression to apply to the catalog search.</param>
    /// <param name="owner">The owner of the catalogs.</param>
    /// <param name="page">The page number for pagination (optional).</param>
    /// <param name="pageSize">The page size for pagination (optional).</param>
    /// <returns>A <see cref="BaseResponse{IEnumerable{CatalogDto}}"/> containing a list of matching catalogs.</returns>
    Task<BaseResponse<IEnumerable<CatalogDto>>> GetCatalogs(Expression<Func<Catalog, bool>> filter, string owner, int? page, int? pageSize);

    /// <summary>
    /// Retrieves a catalog by its identifier.
    /// </summary>
    /// <param name="catalogId">The identifier of the catalog to retrieve.</param>
    /// <param name="owner">The owner of the catalog.</param>
    /// <returns>A <see cref="BaseResponse{CatalogDto}"/> containing the catalog details.</returns>
    Task<BaseResponse<CatalogDto>> GetCatalogById(string catalogId, string owner);
}