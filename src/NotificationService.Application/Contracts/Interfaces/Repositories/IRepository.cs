using System.Linq.Expressions;
using NotificationService.Application.Common.Models;
using NotificationService.SharedKernel.Domain;

namespace NotificationService.Application.Contracts.Interfaces.Repositories;

public interface IRepository<TEntity> where TEntity : EntityBase
{
    Task<(IEnumerable<TEntity>, Pagination)> FindAsync(Expression<Func<TEntity, bool>> filter, FilterOptions filterOptions);
    IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filter);
    Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filter);
    Task<TEntity> InsertOneAsync(TEntity entity);
    Task InsertManyAsync(ICollection<TEntity> entities);
    Task<bool> UpdateOneByIdAsync(string id, TEntity entity);
    Task DeleteOneAsync(Expression<Func<TEntity, bool>> filter, bool hardDelete = false);
    Task DeleteManyAsync(Expression<Func<TEntity, bool>> filter, bool hardDelete = false);
    Task UploadFileAsync(System.IO.Stream file, string fileName);
    Task<byte[]> GetFileByNameAsync(string fileName);
    Task<byte[]> GetFileByIdAsync(string id);
}