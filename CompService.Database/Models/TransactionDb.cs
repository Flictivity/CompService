using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CompService.Database.Models;

public class TransactionDb
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string TransactionId { get; set; } = null!;
    
    [BsonDateTimeOptions(Kind =DateTimeKind.Local)]
    public DateTime TransactionTime { get; set; }
    public double ArrivalMoney { get; set; }
    public double ExpenseMoney { get; set; }
    public int PaymentMethod { get; set; }
    public int TransactionBasis { get; set; }
    public OrderDb? Order { get; set; }
    public UserDb User { get; set; } = null!;
    public string? Comment { get; set; } 
}