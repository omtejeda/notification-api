using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using NotificationService.Infrastructure.Repositories.Helpers;
using LinqKit;
using NotificationService.Application.Contracts.Interfaces.Repositories;
using NotificationService.SharedKernel.Interfaces;
using NotificationService.SharedKernel.Domain;
using NotificationService.Application.Common.Models;

namespace NotificationService.Infrastructure.Repositories;

public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
{
    private readonly IMongoCollection<TEntity> _collection;
    private readonly IGridFSBucket _bucket;
    private readonly IDateTimeService _dateTimeService;

    public GenericRepository(
        IMongoDatabase database,
        IDateTimeService dateTimeService)
    {
        var collectionName = GetCollectionName(typeof(TEntity));
        
        _collection = database.GetCollection<TEntity>(collectionName);
        _bucket = new GridFSBucket(database, new GridFSBucketOptions { BucketName = collectionName });
        _dateTimeService = dateTimeService;
    }

    public async Task<(IEnumerable<TEntity>, Pagination)> FindAsync(Expression<Func<TEntity, bool>> filter, FilterOptions filterOptions) 
    {
        var query = GetFilteredCollection(filter);

        if (filterOptions.SortFields.Any())
            query = query.Sort(GetSortDefinition(filterOptions.SortFields));
        
        var totalCount = await query.CountDocumentsAsync();

        var documents = await query.Paginate(
            filterOptions.Page,
            filterOptions.PageSize)
            .ToListAsync();

        var pagination = new Pagination(
            filterOptions.Page,
            filterOptions.PageSize,
            documents.Count,
            (int)totalCount);

        return (documents, pagination);
    }

    public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filter)
    {
        return GetFilteredCollection(filter).ToEnumerable();
    }

    public async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filter)
    {
        var result = await GetFilteredCollection(filter).FirstOrDefaultAsync();
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
        var now = _dateTimeService.UtcToLocalTime;
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
        var now = _dateTimeService.UtcToLocalTime;
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
        var now = _dateTimeService.UtcToLocalTime;

        if (isUpdate)
            entity.ModifiedOn = now;
        else
            entity.CreatedOn ??= now;
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

        entity.ModifiedOn = _dateTimeService.UtcToLocalTime;

        var result = await _collection.ReplaceOneAsync(x => x.Id == id, entity);
        return result.IsAcknowledged && result.ModifiedCount == 1;
    }

    private static SortDefinition<TEntity> GetSortDefinition(IReadOnlyList<string> sortFields)
    {
        const char descendingPrefix = '-';

        var sortDefinitionBuilder = sortFields.Select(sort =>
        {
            var isDescending = sort.StartsWith(descendingPrefix);
            var fieldName = isDescending ? sort[1..] : sort;

            return isDescending
                ? Builders<TEntity>.Sort.Descending(fieldName)
                : Builders<TEntity>.Sort.Ascending(fieldName);
        });
        
        return Builders<TEntity>.Sort.Combine(sortDefinitionBuilder);
    }

    private IFindFluent<TEntity, TEntity> GetFilteredCollection(Expression<Func<TEntity, bool>> filter)
    {
        var nonDeletedRecords = PredicateBuilder.New<TEntity>()
            .And(x => x.Deleted != true);

        var combinedFilter = filter != null
        ? nonDeletedRecords.And(filter).Expand()
        : nonDeletedRecords;

        return _collection.Find(combinedFilter);
    }

    public static string GetCollectionName(Type entityType)
    {
        var entityName = entityType.Name;
        var pluralSuffix = "s";

        return entityName.EndsWith(pluralSuffix, StringComparison.OrdinalIgnoreCase)
        ? entityName
        : entityName + pluralSuffix;
    }
}