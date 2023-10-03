using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CompService.Database.Models;

public class SourceDb
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string SourceId { get; set; } = null!;
    public string Name { get; set; } = null!;
}