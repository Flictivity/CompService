using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CompService.Database.Models;

public class SparePartDb
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string SparePartId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Article { get; set; } = null!;
    public string CategoryId { get; set; } = null!;
    public int Count { get; set; }
    public double PurchasePrice { get; set; }
    public double RetailPrice { get; set; }
}