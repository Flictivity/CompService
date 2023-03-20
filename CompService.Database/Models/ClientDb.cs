using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CompService.Database.Models;

public class ClientDb
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ClientId { get; set; } = null!;

    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }

    public SourceDb? Source { get; set; }
}