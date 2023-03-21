using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Core.Settings;
using CompService.Database.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CompService.Database.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly IMongoCollection<OrderDb> _orders;
    private readonly ILogger<OrderRepository> _logger;

    public OrderRepository(IOptions<DatabaseConnectionSettings> databaseConnectionSettings,
        ILogger<OrderRepository> logger)
    {
        _logger = logger;
        
        var mongoClient = new MongoClient(
            databaseConnectionSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            databaseConnectionSettings.Value.DatabaseName);

        _orders = mongoDatabase.GetCollection<OrderDb>(
            databaseConnectionSettings.Value.OrdersCollectionName);
    }

    public async Task Create(Order order)
    {
        await _orders.InsertOneAsync(EntityConverter.ConvertOrder(order));
    }

    public async Task<Order?> GetOrderById(string? id)
    {
        var res = (await _orders.FindAsync(x => x.OrderId == id)).FirstOrDefault();
        return res is null
            ? null
            : EntityConverter.ConvertOrder(res);
    }

    public async Task UpdateOrder(Order currentOrder, Order newOrder)
    {
        var newDbOrder = new OrderDb
        {
            OrderId = currentOrder.OrderId,
            OrderDate = newOrder.OrderDate,
            Client = EntityConverter.ConvertClient(newOrder.Client),
            Defect = EntityConverter.ConvertDefect(newOrder.Defect),
            Appearance = EntityConverter.ConvertAppearance(newOrder.Appearance),
            DeviceType = EntityConverter.ConvertDeviceType(newOrder.DeviceType),
            Brand = EntityConverter.ConvertBrand(newOrder.Brand),
            Model = newOrder.Model,
            DevicePassword = newOrder.DevicePassword,
            Operator = EntityConverter.ConvertUser(newOrder.Operator),
            Master = EntityConverter.ConvertUser(newOrder.Master),
            Status = (int) newOrder.Status,
            SpareParts = newOrder.SpareParts?.Select(EntityConverter.ConvertSparePart).ToList(),
            Facilities = newOrder.Facilities?.Select(EntityConverter.ConvertFacility).ToList()
        };
        
        _logger.LogInformation("Данные в таблице успешно изменены");
        await _orders.ReplaceOneAsync(x => x.OrderId == currentOrder.OrderId,newDbOrder);
    }

    public async Task<IEnumerable<Order>> GetAllOrders()
    {
        var orders = (await _orders.FindAsync(x => true)).ToList();
        return orders.Select(EntityConverter.ConvertOrder).ToList();
    }
}