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

    public async Task<BaseResult> Create(Order order)
    {
        await _orderRepository.Create(order);
        return new BaseResult {Success = true};
    }

    public async Task<Order?> GetOrderById(string? id)
    {
        return await _orderRepository.GetOrderById(id);
    }

    public async Task<BaseResult> UpdateOrder(Order currentOrder, Order newOrder)
    {
        await _orderRepository.UpdateOrder(currentOrder, newOrder);
        return new BaseResult {Success = true};
    }

    public async Task<IEnumerable<Order>> GetAllOrders()
    {
        return await _orderRepository.GetAllOrders();
    }
}