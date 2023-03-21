using CompService.Core.Models;
using CompService.Core.Results;

namespace CompService.Core.Services;

public interface ITransactionService
{
    public Task<BaseResult> Create(Transaction newTransaction);
    public Task<Transaction?> GetTransactionById(string? id);
    public Task<BaseResult> UpdateTransaction(Transaction currentTransaction, Transaction newTransaction);
    public Task<TransactionListDataResult> GetAllTransactions();
    public Task<TransactionListDataResult> GetAllTransactionsForPeriod(DateTime periodStart, DateTime periodEnd);
}