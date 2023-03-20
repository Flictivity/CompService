using CompService.Core.Models;

namespace CompService.Core.Results;

public class TransactionResult : BaseResult
{
    public double Balance { get; set; }
    public double Arrival { get; set; }
    public double Expense { get; set; }
}