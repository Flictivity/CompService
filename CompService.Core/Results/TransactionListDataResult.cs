using CompService.Core.Models;

namespace CompService.Core.Results;

public class TransactionListDataResult : BaseResult
{
    public IEnumerable<Transaction> Items { get; init; } = null!;
    public TransactionResult TransactionResult { get; init; } = null!;
}