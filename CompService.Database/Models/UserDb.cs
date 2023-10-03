using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CompService.Database.Models
{
    public class UserDb
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } = null!;

        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Patronymic { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        public int Role { get; set; }
    }
}