using CompService.Core.Enums;

namespace CompService.Core.Models;

public class Transaction
{
    public string TransactionId { get; set; } = null!;
    public DateTime TransactionTime { get; set; } = DateTime.UtcNow;
    public double ArrivalMoney { get; set; } = 0;
    public double ExpenseMoney { get; set; } = 0;
    public PaymentMethods PaymentMethod { get; set; } = 0;
    public TransactionBasis TransactionBasis { get; set; } = 0;
    public string? OrderId { get; set; }
    public string UserId { get; set; } = null!;
    public string UserSurname { get; set; } = null!;
    
    public string? Comment { get; set; } 
}