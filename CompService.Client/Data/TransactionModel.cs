using CompService.Core.Enums;
using CompService.Core.Models;

namespace CompService.Client.Data;

public class TransactionModel
{
    public double ExpenseMoney { get; set; } = 0;
    public PaymentMethods PaymentMethod { get; set; } = 0;
    public TransactionBasis TransactionBasis { get; set; } = 0;
    public Order? Order { get; set; }
    public string? Comment { get; set; } 
}