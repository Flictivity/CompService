using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Core.Results;

namespace CompService.Core.Services.Impl;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;

    public TransactionService(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }
    
    public async Task<BaseResult> Create(Transaction newTransaction)
    {
        await _transactionRepository.Create(newTransaction);
        return new BaseResult {Success = true};
    }

    public async Task<Transaction?> GetTransactionById(string? id)
    {
        return await _transactionRepository.GetTransactionById(id);
    }

    public async Task<BaseResult> UpdateTransaction(Transaction currentTransaction, Transaction newTransaction)
    {
        await _transactionRepository.UpdateTransaction(currentTransaction, newTransaction);
        return new BaseResult {Success = true};
    }

    public async Task<(IEnumerable<Transaction>, TransactionResult)> GetAllTransactions()
    {
        var res = (await _transactionRepository.GetAllTransactions())
            .OrderBy(x => x.TransactionTime)
            .ToList();

        var arrival = res.Sum(x => x.ArrivalMoney);
        var expense = res.Sum(x => x.ExpenseMoney);
        var balance = new TransactionResult
        {
            Success = true,
            Balance = arrival - expense,
            Arrival = arrival,
            Expense = expense
        };
        
        return (res, balance);
    }

    public async Task<(IEnumerable<Transaction>, TransactionResult)> GetAllTransactionsForPeriod(DateTime periodStart,
        DateTime periodEnd)
    {
        var res = (await _transactionRepository
            .GetAllTransactionsForPeriod(periodStart, periodEnd))
            .OrderBy(x => x.TransactionTime)
            .ToList();
        
        var arrival = res.Sum(x => x.ArrivalMoney);
        var expense = res.Sum(x => x.ExpenseMoney);
        var balance = new TransactionResult
        {
            Success = true,
            Balance = arrival - expense,
            Arrival = arrival,
            Expense = expense
        };
        
        return (res, balance);
    }
}