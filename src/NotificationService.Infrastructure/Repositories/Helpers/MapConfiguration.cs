using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Conventions;
using NotificationService.Domain.Models;
using NotificationService.Application.Features.Senders.Dtos;
using NotificationService.SharedKernel;

namespace NotificationService.Infrastructure.Repositories.Helpers;

public static class MapConfiguration
{
    public static IMongoDatabase InitializeMappings(this IMongoDatabase db)
    {
        ConventionRegistry.Register("camelCase", new ConventionPack { new CamelCaseElementNameConvention() }, t => true);
        ConventionRegistry.Register("enumString", new ConventionPack { new EnumRepresentationConvention(BsonType.String) }, t => true);

        BsonClassMap.RegisterClassMap<EntityBase>(cm =>
        {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);
            cm.MapIdMember(c => c.Id)
                .SetSerializer(new StringSerializer(BsonType.ObjectId))
                .SetIdGenerator(StringObjectIdGenerator.Instance);
        });

        BsonClassMap.RegisterClassMap<Attachment>(cm =>
        {
            cm.AutoMap();
            cm.UnmapMember(c => c.FormFile);
        });

        // DTOs saved as part of Notification entity.

        BsonClassMap.RegisterClassMap<SendEmailRequestDto>();
        BsonClassMap.RegisterClassMap<SendSmsRequestDto>();
        BsonClassMap.RegisterClassMap<SendMessageRequestDto>();
        return db;
    }
}