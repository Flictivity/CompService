using CompService.Core.Enums;
using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Core.Results;

namespace CompService.Core.Services.Impl;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IDevicePlaceService _devicePlaceService;
    private readonly ISparePartRepository _sparePartRepository;

    public OrderService(IOrderRepository orderRepository, IDevicePlaceService devicePlaceService,
        ISparePartRepository sparePartRepository)
    {
        _orderRepository = orderRepository;
        _devicePlaceService = devicePlaceService;
        _sparePartRepository = sparePartRepository;
    }

    public async Task<BaseResult> CreateAsync(Order order)
    {
        var place = await _devicePlaceService.GetPlaceByIdAsync(order.DevicePlaceId);
        if (place is null) return new BaseResult {Success = false};

        await _orderRepository.Create(order);
        place.IsOccupied = true;
        await _devicePlaceService.UpdateDevicePlaceAsync(place, place);

        return new BaseResult {Success = true};
    }

    public async Task<Order?> GetOrderByIdAsync(string id)
    {
        return await _orderRepository.GetOrderById(id);
    }

    public async Task<int> GetMasterOrdersCount(string masterId)
    {
        return await _orderRepository.GetMasterOrdersCount(masterId);
    }

    public async Task<BaseResult> UpdateOrderAsync(Order currentOrder, Order newOrder)
    {
        var placeInDbOrder = await _devicePlaceService.GetPlaceByIdAsync(currentOrder.DevicePlaceId);
        if (placeInDbOrder is null) return new BaseResult {Success = false};
        
        var placeInNewOrder = await _devicePlaceService.GetPlaceByIdAsync(newOrder.DevicePlaceId);
        if (placeInNewOrder is null) return new BaseResult {Success = false};
        

        if (!placeInNewOrder.IsOccupied)
        {
            placeInNewOrder.IsOccupied = true;
        }
        
        if (newOrder.Status is OrdersStatuses.Closed or OrdersStatuses.ClosedWithProblems)
        {
            placeInDbOrder.IsOccupied = false;
            placeInNewOrder.IsOccupied = false;
        }

        if (placeInDbOrder.PlaceId != placeInNewOrder.PlaceId)
        {
            placeInDbOrder.IsOccupied = false;
        }

        await _devicePlaceService.UpdateDevicePlaceAsync(placeInDbOrder, placeInDbOrder);
        await _devicePlaceService.UpdateDevicePlaceAsync(placeInNewOrder, placeInNewOrder);

        await _orderRepository.UpdateOrder(currentOrder, newOrder);
        return new BaseResult {Success = true};
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _orderRepository.GetAllOrders();
    }

    public async Task<ListDataResult<OrderTableModel>> GetAllOrdersForTableAsync(int itemCount,
        int pageNum, User user, string searchText, string field, bool desc)
    {
        return await _orderRepository.GetAllOrdersForTable(itemCount, pageNum, user, searchText, field, desc);
    }

    public async Task<BaseResult> RemoveSparePart(string orderId, List<string> sparePartIds)
    {
        var order = await _orderRepository.GetOrderById(orderId);

        if (order is null)
            return new BaseResult {Success = false, Message = "Заказ не найден"};

        foreach (var sparePartId in sparePartIds)
        {
            var sparePart = await _sparePartRepository.GetSparePartById(sparePartId);

            if (sparePart is null)
                return new BaseResult {Success = false, Message = "Запчасть не найдена"};

            var orderSparePart = order.SpareParts?.FirstOrDefault(x =>
                x.Item.SparePartId == sparePartId);

            sparePart.Count += orderSparePart!.ItemCount;

            order.SpareParts?.Remove(orderSparePart);

            await _sparePartRepository.UpdateSparePart(sparePart, sparePart);
        }

        await _orderRepository.UpdateOrder(order, order);

        return new BaseResult {Success = true};
    }
}