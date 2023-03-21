﻿using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Core.Settings;
using CompService.Database.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CompService.Database.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly IMongoCollection<TransactionDb> _transactions;
    private readonly ILogger<ITransactionRepository> _logger;

    public TransactionRepository(IOptions<DatabaseConnectionSettings> databaseConnectionSettings,
        ILogger<ITransactionRepository> logger)
    {
        var mongoClient = new MongoClient(
            databaseConnectionSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            databaseConnectionSettings.Value.DatabaseName);

        _transactions = mongoDatabase.GetCollection<TransactionDb>(
            databaseConnectionSettings.Value.TransactionsCollectionName);
        _logger = logger;
    }

    public async Task Create(Transaction newTransaction)
    {
        var newTransactionDb = EntityConverter.ConvertTransaction(newTransaction);
        await _transactions.InsertOneAsync(newTransactionDb);
    }

    public async Task<Transaction?> GetTransactionById(string? id)
    {
        var res = (await _transactions.FindAsync(x => x.TransactionId == id))
            .FirstOrDefault();
        return res is null ? null : EntityConverter.ConvertTransaction(res);
    }

    public async Task UpdateTransaction(Transaction currentTransaction, Transaction newTransaction)
    {
        var newTransactionDb = new TransactionDb
        {
            TransactionId = currentTransaction.TransactionId,
            TransactionTime = newTransaction.TransactionTime,
            ArrivalMoney = newTransaction.ArrivalMoney,
            ExpenseMoney = newTransaction.ExpenseMoney,
            PaymentMethod = (int) newTransaction.PaymentMethod,
            TransactionBasis = (int) newTransaction.TransactionBasis,
            Order = newTransaction.Order is null
                ? null
                : new OrderDb
                {
                    OrderId = newTransaction.Order.OrderId,
                    OrderDate = newTransaction.Order.OrderDate
                },
            User = new UserDb
            {
                UserId = newTransaction.User.UserId,
                Name = newTransaction.User.Name,
                Surname = newTransaction.User.Surname,
                Patronymic = newTransaction.User.Patronymic,
                Email = newTransaction.User.Email,
                Password = newTransaction.User.Password,
                PhoneNumber = newTransaction.User.PhoneNumber,
                Role = (int) newTransaction.User.Roles
            },
            Comment = newTransaction.Comment
        };

        await _transactions
            .ReplaceOneAsync(x => x.TransactionId == currentTransaction.TransactionId, newTransactionDb);
    }

    public async Task<IEnumerable<Transaction>> GetAllTransactions()
    {
        var transactions = (await _transactions.FindAsync(x => true)).ToList();

        return transactions.Select(EntityConverter.ConvertTransaction).ToList();
    }
}