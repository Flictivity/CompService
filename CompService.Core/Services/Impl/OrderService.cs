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
        int pageNum, string searchText, string field, bool desc)
    {
        return await _orderRepository.GetAllOrdersForTable(itemCount, pageNum, searchText, field, desc);
    }

    public async Task<BaseResult> AddSparePart(string orderId, string sparePartId, int count, double discount,
        bool isEdit)
    {
        var order = await _orderRepository.GetOrderById(orderId);
        var sparePart = await _sparePartRepository.GetSparePartById(sparePartId);

        if (order is null)
            return new BaseResult {Success = false, Message = "Заказ не найден"};


        if (sparePart is null)
            return new BaseResult {Success = false, Message = "Запчасть не найдена"};

        var oldSparePart = order.SpareParts?.FirstOrDefault(x =>
            x.Item.SparePartId == sparePartId);

        var model = new OrderListModel<SparePart>
        {
            Item = sparePart,
            Count = count,
            Discount = discount,
            Sum = sparePart.RetailPrice * count - sparePart.RetailPrice * count * discount * 0.01
        };

        if (sparePart.Count - count < 0)
            return new BaseResult {Success = false, Message = "Недостаточно запчастей на складе"};

        if (!isEdit)
        {
            if (oldSparePart is not null)
                return new BaseResult
                {
                    Success = false,
                    Message = "Такая запчасть уже добавлена. Пожалуйста поменяйте количество"
                };
            await _orderRepository.AddSparePart(order, model);

            if (sparePart.Count - count < 0)
            {
                sparePart.Count = 0;
            }
            else
            {
                sparePart.Count -= count;
            }

            await _sparePartRepository.UpdateSparePart(sparePart, sparePart);
        }
        else
        {
            var oldCount = oldSparePart!.Count;
            await _orderRepository.UpdateSparePart(order, model);

            if (sparePart.Count + oldCount - count < 0)
            {
                sparePart.Count = 0;
            }
            else
            {
                sparePart.Count = sparePart.Count + oldCount - count;
            }

            await _sparePartRepository.UpdateSparePart(sparePart, sparePart);
        }

        return new BaseResult {Success = true};
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

            sparePart.Count += orderSparePart!.Count;

            order.SpareParts?.Remove(orderSparePart);

            await _sparePartRepository.UpdateSparePart(sparePart, sparePart);
        }

        await _orderRepository.UpdateOrder(order, order);

        return new BaseResult {Success = true};
    }
}