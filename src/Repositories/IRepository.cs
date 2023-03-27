using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NotificationService.Entities;

namespace NotificationService.Repositories
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<(IEnumerable<TEntity>, Pagination)> FindAsync(Expression<Func<TEntity, bool>> filter, int? page = null, int? pageSize = null);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filter);
        IEnumerable<TProjected> FindProjection<TProjected>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TProjected>> projection);
        Task<IEnumerable<TProjected>> FindProjectionAsync<TProjected>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TProjected>> projection);
        Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filter);
        Task<TEntity> InsertOneAsync(TEntity entity);
        Task InsertManyAsync(ICollection<TEntity> entities);
        Task DeleteOneAsync(Expression<Func<TEntity, bool>> filter, bool hardDelete = false);
        Task DeleteManyAsync(Expression<Func<TEntity, bool>> filter, bool hardDelete = false);
    }
}