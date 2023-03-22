using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CompService.Database.Models;

public class UserVerificationDb
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string VerificationId { get; set; } = null!;

    public string Code { get; set; } = null!;
    public bool IsActual { get; set; } = true;
    public DateTime ExpyreTime { get; set; }
    public string UserId { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
}