using CompService.Core.Enums;
using CompService.Core.Extensions;
using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Core.Results;

namespace CompService.Core.Services.Impl;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<BaseResult> CreateAsync(Order order)
    {
        await _orderRepository.Create(order);
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
        await _orderRepository.UpdateOrder(currentOrder, newOrder);
        return new BaseResult {Success = true};
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _orderRepository.GetAllOrders();
    }

    public async Task<IEnumerable<OrderTableModel>> GetAllOrdersForTableAsync()
    {
        return await _orderRepository.GetAllOrdersForTable();
    }
}