using CompService.Core.Models;

namespace CompService.Core.Results;

public class TransactionResult : BaseResult
{
    public double Profit { get; set; }
    public double Arrival { get; set; }
    public double Expense { get; set; }
}