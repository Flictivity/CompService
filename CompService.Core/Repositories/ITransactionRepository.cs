﻿
using CompService.Core.Models;

namespace CompService.Core.Repositories;

public interface ITransactionRepository
{
    public Task Create(Transaction newTransaction);
    public Task<Transaction?> GetTransactionById(string id);
    public Task<Transaction?> GetTransactionByOrder(string orderId);
    public Task UpdateTransaction(Transaction currentTransaction, Transaction newTransaction);
    public Task<IEnumerable<Transaction>> GetAllTransactions();
}