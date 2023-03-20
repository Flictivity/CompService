using CompService.Core.Enums;

namespace CompService.Core.Models;

public class Transaction
{
    public string TransactionId { get; set; } = null!;
    public DateTime TransactionTime { get; set; } = DateTime.UtcNow;
    public double ArrivalMoney { get; set; } = 0;
    public double ExpenseMoney { get; set; } = 0;
    public PaymentMethods PaymentMehod { get; set; } = 0;
    public TransactionBasis TransactionBasis { get; set; } = 0;
    public Order? Order { get; set; }
    public User User { get; set; } = null!;
}