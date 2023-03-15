using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CompService.Database.Models;

public class BrandDb
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string BrandId { get; set; } = null!;
    public string Name { get; set; } = null!;
}