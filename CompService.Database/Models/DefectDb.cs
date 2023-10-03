using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CompService.Database.Models;

public class DefectDb
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string DefectId { get; set; } = null!;
    public string Name { get; set; } = null!;
}