using System.Linq.Expressions;
using NotificationService.Application.Common.Models;
using NotificationService.SharedKernel.Domain;

namespace NotificationService.Application.Contracts.Persistence;

/// <summary>
/// Defines a generic repository interface for performing CRUD operations on entities, 
/// including file upload and retrieval operations, as well as soft and hard deletes.
/// </summary>
/// <typeparam name="TEntity">The type of entity managed by the repository.</typeparam>
public interface IRepository<TEntity> where TEntity : EntityBase
{
    /// <summary>
    /// Finds entities that match a given filter expression and returns them along with pagination details.
    /// </summary>
    /// <param name="filter">The expression used to filter entities.</param>
    /// <param name="filterOptions">Options to control filtering and pagination.</param>
    /// <returns>A tuple containing the filtered entities and pagination details.</returns>
    Task<(IEnumerable<TEntity>, Pagination)> FindAsync(Expression<Func<TEntity, bool>> filter, FilterOptions filterOptions);

    /// <summary>
    /// Finds entities that match a given filter expression.
    /// </summary>
    /// <param name="filter">The expression used to filter entities.</param>
    /// <returns>An enumerable collection of the filtered entities.</returns>
    IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filter);

    /// <summary>
    /// Finds a single entity that matches a given filter expression asynchronously.
    /// </summary>
    /// <param name="filter">The expression used to filter the entity.</param>
    /// <returns>The entity that matches the filter, or null if no match is found.</returns>
    Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filter);

    /// <summary>
    /// Inserts a single entity asynchronously into the repository.
    /// </summary>
    /// <param name="entity">The entity to insert.</param>
    /// <returns>The inserted entity.</returns>
    Task<TEntity> InsertOneAsync(TEntity entity);

    /// <summary>
    /// Inserts multiple entities asynchronously into the repository.
    /// </summary>
    /// <param name="entities">A collection of entities to insert.</param>
    Task InsertManyAsync(ICollection<TEntity> entities);

    /// <summary>
    /// Updates an entity in the repository by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity to update.</param>
    /// <param name="entity">The updated entity.</param>
    /// <returns>A boolean value indicating whether the update was successful.</returns>
    Task<bool> UpdateOneByIdAsync(string id, TEntity entity);

    /// <summary>
    /// Deletes a single entity that matches a given filter expression asynchronously.
    /// </summary>
    /// <param name="filter">The expression used to filter the entity for deletion.</param>
    /// <param name="hardDelete">If true, the entity is permanently deleted; otherwise, it is soft deleted.</param>
    Task DeleteOneAsync(Expression<Func<TEntity, bool>> filter, bool hardDelete = false);

    /// <summary>
    /// Deletes multiple entities that match a given filter expression asynchronously.
    /// </summary>
    /// <param name="filter">The expression used to filter the entities for deletion.</param>
    /// <param name="hardDelete">If true, the entities are permanently deleted; otherwise, they are soft deleted.</param>
    Task DeleteManyAsync(Expression<Func<TEntity, bool>> filter, bool hardDelete = false);

    /// <summary>
    /// Uploads a file asynchronously to the repository.
    /// </summary>
    /// <param name="file">The stream of the file to upload.</param>
    /// <param name="fileName">The name of the file being uploaded.</param>
    Task UploadFileAsync(System.IO.Stream file, string fileName);

    /// <summary>
    /// Retrieves a file by its name asynchronously from the repository.
    /// </summary>
    /// <param name="fileName">The name of the file to retrieve.</param>
    /// <returns>A byte array representing the file content.</returns>
    Task<byte[]> GetFileByNameAsync(string fileName);

    /// <summary>
    /// Retrieves a file by its identifier asynchronously from the repository.
    /// </summary>
    /// <param name="id">The identifier of the file to retrieve.</param>
    /// <returns>A byte array representing the file content.</returns>
    Task<byte[]> GetFileByIdAsync(string id);
}