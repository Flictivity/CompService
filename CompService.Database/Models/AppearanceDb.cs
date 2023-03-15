using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CompService.Database.Models;

public class AppearanceDb
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string AppearanceId { get; set; } = null!;
    public string Name { get; set; } = null!;
}