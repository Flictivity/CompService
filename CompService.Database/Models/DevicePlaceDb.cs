using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CompService.Database.Models;

public class DevicePlaceDb
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string PlaceId { get; set; } = null!;
    public string Info { get; set; } = null!;
    public OrderDb? Order { get; set; } = null!;
    public bool IsOccupied { get; set; }
}