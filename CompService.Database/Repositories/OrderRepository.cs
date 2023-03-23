using CompService.Core.Enums;
using CompService.Core.Extensions;
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
    private readonly ISparePartRepository _sparePartRepository;
    private readonly IFacilityRepository _facilityRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IReferenceRepository<Defect> _defectRepository;
    private readonly IUserRepository _userRepository;
    private readonly IDevicePlaceRepository _devicePlaceRepository;

    public OrderRepository(IOptions<DatabaseConnectionSettings> databaseConnectionSettings,
        ILogger<OrderRepository> logger, ISparePartRepository sparePartRepository,
        IFacilityRepository facilityRepository, IClientRepository clientRepository,
        IReferenceRepository<Defect> defectRepository, IUserRepository userRepository,
        IDevicePlaceRepository devicePlaceRepository)
    {
        _logger = logger;
        _sparePartRepository = sparePartRepository;
        _facilityRepository = facilityRepository;
        _clientRepository = clientRepository;
        _defectRepository = defectRepository;
        _userRepository = userRepository;
        _devicePlaceRepository = devicePlaceRepository;

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
        if (res is null)
        {
            return null;
        }

        var order = EntityConverter.ConvertOrder(res);
        var spareParts = new List<OrderListModel<SparePart>>();
        var facilities = new List<OrderListModel<Facility>>();
        if (res.SpareParts is not null)
        {
            foreach (var sparePart in res.SpareParts)
            {
                var newSparePart = new OrderListModel<SparePart>
                {
                    Item = await _sparePartRepository.GetSparePartById(sparePart.Item) ?? new SparePart(),
                    Count = sparePart.Count,
                    Discount = sparePart.Discount,
                    Sum = sparePart.Sum
                };
                spareParts.Add(newSparePart);
            }
        }

        if (res.Facilities is not null)
        {
            foreach (var facility in res.Facilities)
            {
                var newFacility = new OrderListModel<Facility>
                {
                    Item = await _facilityRepository.GetFacilityById(facility.Item) ?? new Facility(),
                    Count = facility.Count,
                    Discount = facility.Discount,
                    Sum = facility.Sum
                };
                facilities.Add(newFacility);
            }
        }

        order.Facilities = facilities;
        order.SpareParts = spareParts;
        return order;
    }

    public async Task<int> GetMasterOrdersCount(string masterId)
    {
        return (await _orders.FindAsync(x => x.MasterId == masterId)).ToList().Count;
    }

    public async Task UpdateOrder(Order currentOrder, Order newOrder)
    {
        var spareParts = newOrder.SpareParts?
            .Select(sparePart => new OrderListModel<string>
            {
                Item = sparePart.Item.SparePartId,
                Count = sparePart.Count,
                Discount = sparePart.Discount,
                Sum = sparePart.Sum
            })
            .ToList();
        var facilities = newOrder.Facilities?
            .Select(facility => new OrderListModel<string>
            {
                Item = facility.Item.FacilityId,
                Count = facility.Count,
                Discount = facility.Discount,
                Sum = facility.Sum
            })
            .ToList();
        var newDbOrder = new OrderDb
        {
            OrderId = currentOrder.OrderId,
            OrderDate = newOrder.OrderDate,
            ClientId = newOrder.ClientId,
            DefectId = newOrder.DefectId,
            AppearanceId = newOrder.AppearanceId,
            DeviceTypeId = newOrder.DeviceTypeId,
            BrandId = newOrder.BrandId,
            Model = newOrder.Model,
            DevicePassword = newOrder.DevicePassword,
            OperatorId = newOrder.OperatorId,
            MasterId = newOrder.MasterId,
            Status = (int) newOrder.Status,
            RepairType = (int) newOrder.RepairType,
            SpareParts = spareParts,
            Facilities = facilities,
            Money = newOrder.Money,
            DevicePlaceId = newOrder.DevicePlaceId
        };

        _logger.LogInformation("Данные в таблице успешно изменены");
        await _orders.ReplaceOneAsync(x => x.OrderId == currentOrder.OrderId, newDbOrder);
    }

    public async Task<IEnumerable<Order>> GetAllOrders()
    {
        var res = new List<Order>();
        var orders = (await _orders.FindAsync(x => true)).ToList();

        foreach (var order in orders)
        {
            var newOrder = EntityConverter.ConvertOrder(order);
            var spareParts = new List<OrderListModel<SparePart>>();
            var facilities = new List<OrderListModel<Facility>>();
            if (order.SpareParts is not null)
            {
                foreach (var sparePart in order.SpareParts)
                {
                    var newSparePart = new OrderListModel<SparePart>
                    {
                        Item = await _sparePartRepository.GetSparePartById(sparePart.Item) ?? new SparePart(),
                        Count = sparePart.Count,
                        Discount = sparePart.Discount,
                        Sum = sparePart.Sum
                    };
                    spareParts.Add(newSparePart);
                }
            }

            if (order.Facilities is not null)
            {
                foreach (var facility in order.Facilities)
                {
                    var newFacility = new OrderListModel<Facility>
                    {
                        Item = await _facilityRepository.GetFacilityById(facility.Item) ?? new Facility(),
                        Count = facility.Count,
                        Discount = facility.Discount,
                        Sum = facility.Sum
                    };
                    facilities.Add(newFacility);
                }
            }

            newOrder.Facilities = facilities;
            newOrder.SpareParts = spareParts;
            res.Add(newOrder);
        }

        return res;
    }

    public async Task<IEnumerable<OrderTableModel>> GetAllOrdersForTable()
    {
        var orders = (await _orders.FindAsync(x => true)).ToList();
        var res = new List<OrderTableModel>();
        foreach (var order in orders)
        {
            var client = await _clientRepository.GetClientById(order.ClientId);
            var orderOperator = await _userRepository.GetUserById(order.OperatorId);
            var master = await _userRepository.GetUserById(order.MasterId);
            var model = new OrderTableModel
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                Status = (OrdersStatuses) order.Status,
                ClientSurname = client?.Surname!,
                ClientPhoneNumber = client?.PhoneNumber ?? string.Empty,
                Defect = (await _defectRepository.GetReferenceById(order.DefectId))?.Name ?? string.Empty,
                OperatorName = $"{orderOperator?.Surname} {orderOperator?.Name[0]}. {orderOperator?.Patronymic[0]}",
                MasterName = $"{master?.Surname} {master?.Name[0]}. {master?.Patronymic[0]}",
                Place = (await _devicePlaceRepository.GetPlaceById(order.DevicePlaceId))?.Info ?? string.Empty,
                Sum = order.Money
            };

            res.Add(model);
        }

        return res;
    }
}