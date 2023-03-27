using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using NotificationService.Entities;
using NotificationService.Repositories.Helpers;
using LinqKit;

namespace NotificationService.Repositories
{
    public class MongoRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly IMongoCollection<TEntity> _collection;
        private readonly Expression<Func<TEntity, bool>> _nonDeletedRecords;

        public MongoRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<TEntity>($"{typeof(TEntity).Name}s");
            _nonDeletedRecords = PredicateBuilder.New<TEntity>().And(x => x.Deleted != true).Expand();
        }

        public async Task<(IEnumerable<TEntity>, Pagination)> FindAsync(Expression<Func<TEntity, bool>> filter, int? page = null, int? pageSize = null)
        {
            filter = filter.And(_nonDeletedRecords);
            var query = _collection.Find(filter);
            var totalCount = await query.CountDocumentsAsync();

            var documents = await query.Paginate(page, pageSize).ToListAsync();
            var pagination = new Pagination(page, RepositoryHelper.GetPageSizeBasedOnLimit(pageSize), documents.Count, (int) totalCount);
            
            return (documents, pagination);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filter)
        {
            filter = filter.And(_nonDeletedRecords);
            return _collection.Find(filter).ToEnumerable();
        }

        public IEnumerable<TProjected> FindProjection<TProjected>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TProjected>> projection)
        {
            filter = filter.And(_nonDeletedRecords);
            return _collection.Find(filter).Project(projection).ToEnumerable();
        }

        public async Task<IEnumerable<TProjected>> FindProjectionAsync<TProjected>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TProjected>> projection)
        {
            filter = filter.And(_nonDeletedRecords);
            return await _collection.Find(filter).Project(projection).ToListAsync();
        }

        public async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filter)
        {
            filter = filter.And(_nonDeletedRecords);
            var result = await _collection.Find(filter).FirstOrDefaultAsync();
            return result;
        }

        public async Task<TEntity> InsertOneAsync(TEntity entity)
        {
            AddTimestamp(entity);
            await _collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task InsertManyAsync(ICollection<TEntity> entities)
        {
            AddTimestamp(entities);
            await _collection.InsertManyAsync(entities);
        }

        public async Task DeleteOneAsync(Expression<Func<TEntity, bool>> filter, bool hardDelete = false)
        {
            var now = Utils.SystemUtil.GetSystemDate();
            if (hardDelete)
            {
                await _collection.FindOneAndDeleteAsync(filter);
            }
            else
            {
                var update = Builders<TEntity>.Update
                            .Set(x => x.Deleted, true)
                            .Set(x => x.ModifiedOn, now);

                await _collection.UpdateOneAsync(filter, update);
            }
        }

        public async Task DeleteManyAsync(Expression<Func<TEntity, bool>> filter, bool hardDelete = false)
        {
            var now = Utils.SystemUtil.GetSystemDate();
            if (hardDelete)
            {
                await _collection.DeleteManyAsync(filter);
            }
            else
            {
                var update = Builders<TEntity>.Update
                            .Set(x => x.Deleted, true)
                            .Set(x => x.ModifiedOn, now);

                await _collection.UpdateManyAsync(filter, update);
            }
        }

        private void AddTimestamp(TEntity entity, bool isUpdate = false)
        {
            var now = Utils.SystemUtil.GetSystemDate();
            
            if (isUpdate) entity.ModifiedOn = now;
            else entity.CreatedOn ??= now;
        }

        private void AddTimestamp(ICollection<TEntity> entities, bool isUpdate = false)
        {
            foreach (var entity in entities)
                AddTimestamp(entity, isUpdate);
        }
    }
}