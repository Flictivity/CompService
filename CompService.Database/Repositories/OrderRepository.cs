using System.Linq.Expressions;
using CompService.Core.Enums;
using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Core.Results;
using CompService.Core.Settings;
using CompService.Database.Extensions;
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

    public async Task<IEnumerable<OrderListModel<SparePart>>> GetOrderSpareParts(string id)
    {
        var spareParts = new List<OrderListModel<SparePart>>();
        var res = (await _orders.FindAsync(x => x.OrderId == id)).FirstOrDefault();
        if (res is null) return spareParts;
        if (res.SpareParts is null) return spareParts;

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

        return spareParts;
    }

    public async Task<IEnumerable<OrderListModel<Facility>>> GetOrderFacilities(string id)
    {
        var facilities = new List<OrderListModel<Facility>>();
        var res = (await _orders.FindAsync(x => x.OrderId == id)).FirstOrDefault();
        if (res is null) return facilities;
        if (res.Facilities is null) return facilities;

        foreach (var facility in res.Facilities)
        {
            var newSparePart = new OrderListModel<Facility>
            {
                Item = await _facilityRepository.GetFacilityById(facility.Item) ?? new Facility(),
                Count = facility.Count,
                Discount = facility.Discount,
                Sum = facility.Sum
            };
            facilities.Add(newSparePart);
        }

        return facilities;
    }

    public async Task<int> GetMasterOrdersCount(string masterId)
    {
        return (await _orders
                .FindAsync(x => x.MasterId == masterId && (x.Status != (int) OrdersStatuses.Closed ||
                                                           x.Status != (int) OrdersStatuses.Finished ||
                                                           x.Status != (int) OrdersStatuses.ClosedWithProblems)))
            .ToList().Count;
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

        var newDbOrder = EntityConverter.ConvertOrder(newOrder);
        newDbOrder.Facilities = facilities;
        newDbOrder.SpareParts = spareParts;
        newDbOrder.OrderId = currentOrder.OrderId;

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

    public async Task<ListDataResult<OrderTableModel>> GetAllOrdersForTable(int itemCount, int pageNum,
        string searchText, string field, bool desc = false)
    {
        var orderProperty = GetOrderProperty(field);
        var search = searchText.ToLower();

        List<OrderDb> filtered = new();
        if (search != "")
        {
            using var cursor = await _orders.FindAsync(x => true);
            while (await cursor.MoveNextAsync())
            {
                var batch = cursor.Current;
                foreach (var document in batch)
                {
                    var operatorDb = await _userRepository.GetUserById(document.OperatorId);
                    var master = await _userRepository.GetUserById(document.MasterId);
                    var defect = await _defectRepository.GetReferenceById(document.DefectId);
                    var place = await _devicePlaceRepository.GetPlaceById(document.DevicePlaceId);
                    var client = await _clientRepository.GetClientById(document.ClientId);
                    if (string.Join(" ", operatorDb?.Surname, operatorDb?.Name, operatorDb?.Patronymic, master?.Surname,
                            master?.Name, master?.Patronymic, client?.Name, defect?.Name, place?.Info, client?.Surname)
                        .ToLower().Contains(search))
                    {
                        filtered.Add(document);
                    }
                }
            }
        }
        else
        {
            filtered = (await _orders.FindAsync(x => true)).ToList();
        }

        var orders = filtered
            .AsQueryable()
            .OrderBy(orderProperty, desc)
            .Skip(pageNum * itemCount)
            .Take(itemCount)
            .ToList();

        var convertedOrders = new List<OrderTableModel>();
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

            convertedOrders.Add(model);
        }

        return new ListDataResult<OrderTableModel>
        {
            Items = convertedOrders.ToList(),
            TotalItemsCount = filtered.Count
        };
    }

    public async Task AddSparePart(Order order, OrderListModel<SparePart> sparePart)
    {
        order.SpareParts?.Add(sparePart);
        order.Money = order.SpareParts!.Sum(x => x.Item.RetailPrice) + 
                      order.Facilities!.Sum(x => x.Item.Cost);

        await UpdateOrder(order, order);
    }

    public async Task UpdateSparePart(Order order, OrderListModel<SparePart> sparePart)
    {
        if (order.SpareParts is null)
        {
            return;
        }

        var orderSparePart = order.SpareParts.FirstOrDefault(x => 
            x.Item.SparePartId == sparePart.Item.SparePartId);
        
        orderSparePart!.Item = sparePart.Item;
        orderSparePart.Discount = sparePart.Discount;
        orderSparePart.Sum = sparePart.Sum;
        orderSparePart.Count = sparePart.Count;
        
        order.Money = order.SpareParts!.Sum(x => x.Item.RetailPrice) + 
                      order.Facilities!.Sum(x => x.Item.Cost);
        
        await UpdateOrder(order, order);
    }

    private Expression<Func<OrderDb, object>> GetOrderProperty(string field)
    {
        if (field == "")
        {
            return x => x.OrderId;
        }

        return field switch
        {
            "OrderId" => x => x.OrderId,
            "Status" => x => x.Status,
            "Sum" => x => x.Money,
            _ => throw new ArgumentOutOfRangeException(nameof(field), field, null)
        };
    }
}