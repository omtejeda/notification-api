using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using NotificationService.Common.Entities;
using NotificationService.Common.Models;
using NotificationService.Infrastructure.Repositories.Helpers;
using LinqKit;
using NotificationService.Common.Utils;
using NotificationService.Contracts.Interfaces.Repositories;
using System.Linq;

namespace NotificationService.Infrastructure.Repositories
{
    public class MongoRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly IMongoCollection<TEntity> _collection;
        private readonly IGridFSBucket _bucket;
        private readonly Expression<Func<TEntity, bool>> _nonDeletedRecords;

        public MongoRepository(IMongoDatabase database)
        {
            var collectionName = string.Concat(typeof(TEntity).Name, 's');

            _collection = database.GetCollection<TEntity>(collectionName);
            _bucket = new GridFSBucket(database, new GridFSBucketOptions { BucketName = collectionName });

            _nonDeletedRecords = PredicateBuilder.New<TEntity>().And(x => x.Deleted != true).Expand();
        }

        public async Task<(IEnumerable<TEntity>, Pagination)> FindAsync(Expression<Func<TEntity, bool>> filter, int? page = null, int? pageSize = null, IReadOnlyList<string> sortsBy = null) 
        {
            filter = filter.And(_nonDeletedRecords);
            var query = _collection.Find(filter);

            if (sortsBy?.Any() == true)
                query = query.Sort(GetSortDefinition(sortsBy));
            
            var totalCount = await query.CountDocumentsAsync();

            var documents = await query.Paginate(page, pageSize).ToListAsync();
            var pagination = new Pagination(page, RepositoryHelper.GetPageSizeBasedOnLimit(pageSize), documents.Count, (int)totalCount);

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
            var now = SystemUtil.GetSystemDate();
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
            var now = SystemUtil.GetSystemDate();
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
            var now = SystemUtil.GetSystemDate();

            if (isUpdate) entity.ModifiedOn = now;
            else entity.CreatedOn ??= now;
        }

        private void AddTimestamp(ICollection<TEntity> entities, bool isUpdate = false)
        {
            foreach (var entity in entities)
                AddTimestamp(entity, isUpdate);
        }

        public async Task UploadFileAsync(System.IO.Stream file, string fileName)
        {
            using var stream = await _bucket.OpenUploadStreamAsync(fileName);

            file.CopyTo(stream);
            await stream.CloseAsync();
        }

        public async Task<byte[]> GetFileByNameAsync(string fileName)
        {
            return await _bucket.DownloadAsBytesByNameAsync(fileName);
        }

        public async Task<byte[]> GetFileByIdAsync(string id)
        {
            var objectId = new ObjectId(id);
            return await _bucket.DownloadAsBytesAsync(objectId);
        }

        public async Task<bool> UpdateOneByIdAsync(string id, TEntity entity)
        {
            if (id is null || entity is null || entity.Id != id || entity.CreatedOn is null)
                return false;

            entity.ModifiedOn = SystemUtil.GetSystemDate();

            var result = await _collection.ReplaceOneAsync(x => x.Id == id, entity);
            return result.IsAcknowledged && result.ModifiedCount == 1;
        }

        private static SortDefinition<TEntity> GetSortDefinition(IReadOnlyList<string> sorts)
        {
            const char descendingPrefix = '-';

            var sortDefinitions = sorts.Select(sort =>
            {
                var isDescending = sort.StartsWith(descendingPrefix);
                
                var fieldName = isDescending ? sort[1..] : sort;

                return isDescending
                    ? Builders<TEntity>.Sort.Descending(fieldName)
                    : Builders<TEntity>.Sort.Ascending(fieldName);
            });

            return Builders<TEntity>.Sort.Combine(sortDefinitions);
        }
    }
}