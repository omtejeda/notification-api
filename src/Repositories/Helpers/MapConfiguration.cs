using System;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Conventions;
using NotificationService.Entities;
using NotificationService.Dtos.Requests;

namespace NotificationService.Repositories.Helpers
{
    public static class MapConfiguration
    {
        public static IMongoDatabase InitializeMappings(this IMongoDatabase db)
        {
            ConventionRegistry.Register("camelCase", new ConventionPack { new CamelCaseElementNameConvention() }, t => true);
            ConventionRegistry.Register("enumString", new ConventionPack { new EnumRepresentationConvention(BsonType.String) }, t => true);

            BsonClassMap.RegisterClassMap<BaseEntity>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
                cm.MapIdMember(c => c.Id)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId))
                    .SetIdGenerator(StringObjectIdGenerator.Instance);
            });

            // DTOs saved as part of Notification entity.
            BsonClassMap.RegisterClassMap<SendEmailRequestDto>();
            BsonClassMap.RegisterClassMap<SendSmsRequestDto>();
            return db;
        }
    }
}