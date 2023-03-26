using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Core.Results;

namespace CompService.Core.Services.Impl;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IDevicePlaceService _devicePlaceService;

    public OrderService(IOrderRepository orderRepository, IDevicePlaceService devicePlaceService)
    {
        _orderRepository = orderRepository;
        _devicePlaceService = devicePlaceService;
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

    public async Task<IEnumerable<OrderListModel<SparePart>>> GetOrderSparePartsAsync(string id)
    {
        return await _orderRepository.GetOrderSpareParts(id);
    }

    public async Task<int> GetMasterOrdersCount(string masterId)
    {
        return await _orderRepository.GetMasterOrdersCount(masterId);
    }

    public async Task<BaseResult> UpdateOrderAsync(Order currentOrder, Order newOrder)
    {
        await _orderRepository.UpdateOrder(currentOrder, newOrder);
        return new BaseResult {Success = true};
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _orderRepository.GetAllOrders();
    }

    public async Task<ListDataResult<OrderTableModel>> GetAllOrdersForTableAsync(int itemCount,
        int pageNum, string field, bool desc, string searchText)
    {
        return await _orderRepository.GetAllOrdersForTable(itemCount, pageNum, searchText,field,desc);
    }
}