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
    
    public async Task<BaseResult> CreateAsync(Transaction newTransaction)
    {
        await _transactionRepository.Create(newTransaction);
        return new BaseResult {Success = true};
    }

    public async Task<Transaction?> GetTransactionByIdAsync(string id)
    {
        return await _transactionRepository.GetTransactionById(id);
    }

    public async Task<Transaction?> GetTransactionByOrderAsync(string orderId)
    {
        return await _transactionRepository.GetTransactionByOrder(orderId);
    }

    public async Task<BaseResult> UpdateTransactionAsync(Transaction currentTransaction, Transaction newTransaction)
    {
        await _transactionRepository.UpdateTransaction(currentTransaction, newTransaction);
        return new BaseResult {Success = true};
    }

    public async Task<TransactionListDataResult> GetAllTransactionsAsync()
    {
        var res = (await _transactionRepository.GetAllTransactions())
            .OrderBy(x => x.TransactionTime)
            .ToList();

        var arrival = res.Sum(x => x.ArrivalMoney);
        var expense = res.Sum(x => x.ExpenseMoney);
        var balance = new TransactionResult
        {
            Success = true,
            Profit = arrival - expense,
            Arrival = arrival,
            Expense = expense
        };
        
        return new TransactionListDataResult{Success = true, TransactionResult = balance, Items = res};
    }

    public async Task<TransactionListDataResult> GetAllTransactionsForPeriodAsync(DateTime periodStart,
        DateTime periodEnd)
    {
        var res = (await _transactionRepository
            .GetAllTransactions())
            .Where(x => x.TransactionTime >= periodStart && x.TransactionTime <= periodEnd)
            .OrderBy(x => x.TransactionTime)
            .ToList();
        
        var arrival = res.Sum(x => x.ArrivalMoney);
        var expense = res.Sum(x => x.ExpenseMoney);
        var balance = new TransactionResult
        {
            Success = true,
            Profit = Math.Round(arrival - expense, 2),
            Arrival = arrival,
            Expense = expense
        };
        
        return new TransactionListDataResult{Success = true, TransactionResult = balance, Items = res};
    }
}