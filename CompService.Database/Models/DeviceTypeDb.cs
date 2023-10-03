using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CompService.Database.Models;

public class DeviceTypeDb
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string DeviceTypeId { get; set; } = null!;
    public string Name { get; set; } = null!;
}