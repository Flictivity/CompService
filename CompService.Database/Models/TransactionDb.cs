using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CompService.Database.Models;

public class TransactionDb
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string TransactionId { get; set; } = null!;
    public DateTime TransactionTime { get; set; } = DateTime.UtcNow;
    public double ArrivalMoney { get; set; } = 0;
    public double ExpenseMoney { get; set; } = 0;
    public int PaymentMehod { get; set; } = 0;
    public int TransactionBasis { get; set; } = 0;
    public OrderDb? OrderDb { get; set; }
    public UserDb User { get; set; } = null!;
}