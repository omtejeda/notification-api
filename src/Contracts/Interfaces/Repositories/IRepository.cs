using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NotificationService.Core.Common.Entities;

namespace NotificationService.Contracts.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<(IEnumerable<TEntity>, Pagination)> FindAsync(Expression<Func<TEntity, bool>> filter, int? page = null, int? pageSize = null, IReadOnlyList<string> sortsBy = null);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filter);
        IEnumerable<TProjected> FindProjection<TProjected>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TProjected>> projection);
        Task<IEnumerable<TProjected>> FindProjectionAsync<TProjected>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TProjected>> projection);
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
}