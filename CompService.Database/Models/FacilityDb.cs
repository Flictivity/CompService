using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CompService.Database.Models;

public class FacilityDb
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string FacilityId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public double Cost { get; set; }
}
