using CompService.Core.Models;
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
    private readonly IUserRepository _userRepository;

    public TransactionRepository(IOptions<DatabaseConnectionSettings> databaseConnectionSettings,
        ILogger<ITransactionRepository> logger, IUserRepository userRepository)
    {
        var mongoClient = new MongoClient(
            databaseConnectionSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            databaseConnectionSettings.Value.DatabaseName);

        _transactions = mongoDatabase.GetCollection<TransactionDb>(
            databaseConnectionSettings.Value.TransactionsCollectionName);
        _logger = logger;
        _userRepository = userRepository;
    }

    public async Task Create(Transaction newTransaction)
    {
        var newTransactionDb = EntityConverter.ConvertTransaction(newTransaction);
        await _transactions.InsertOneAsync(newTransactionDb);
    }

    public async Task<Transaction?> GetTransactionById(string id)
    {
        var res = (await _transactions.FindAsync(x => x.TransactionId == id))
            .FirstOrDefault();
        if (res is null)
        {
            return null;
        }

        var user = await _userRepository.GetUserById(res.UserId);
        var transaction = EntityConverter.ConvertTransaction(res);
        transaction.UserSurname = user is null ? string.Empty : user.Surname;

        return transaction;
    }

    public async Task<Transaction?> GetTransactionByOrder(string orderId)
    {
        var res = (await _transactions.FindAsync(x => x.OrderId == orderId))
            .FirstOrDefault();
        if (res is null)
        {
            return null;
        }

        var user = await _userRepository.GetUserById(res.UserId);
        var transaction = EntityConverter.ConvertTransaction(res);
        transaction.UserSurname = user is null ? string.Empty : user.Surname;

        return transaction;
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
            OrderId = newTransaction.OrderId,
            UserId = newTransaction.UserId,
            Comment = newTransaction.Comment
        };

        await _transactions
            .ReplaceOneAsync(x => x.TransactionId == currentTransaction.TransactionId, newTransactionDb);
    }

    public async Task<IEnumerable<Transaction>> GetAllTransactions()
    {
        var res = new List<Transaction>();
        var transactions = (await _transactions.FindAsync(x => true)).ToList();

        foreach (var transaction in transactions)
        {
            var user = await _userRepository.GetUserById(transaction.UserId);
            var newTransaction = EntityConverter.ConvertTransaction(transaction);
            newTransaction.UserSurname = user is null ? string.Empty : user.Surname;

            res.Add(newTransaction);
        }

        return res;
    }
}