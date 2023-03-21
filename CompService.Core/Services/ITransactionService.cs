using CompService.Core.Models;
using CompService.Core.Results;

namespace CompService.Core.Services;

public interface ITransactionService
{
    public Task<BaseResult> CreateAsync(Transaction newTransaction);
    public Task<Transaction?> GetTransactionByIdAsync(string? id);
    public Task<BaseResult> UpdateTransactionAsync(Transaction currentTransaction, Transaction newTransaction);
    public Task<TransactionListDataResult> GetAllTransactionsAsync();
    public Task<TransactionListDataResult> GetAllTransactionsForPeriodAsync(DateTime periodStart, DateTime periodEnd);
}