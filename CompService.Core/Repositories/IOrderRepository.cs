using CompService.Core.Models;

namespace CompService.Core.Repositories;

public interface IOrderRepository
{
    public Task Create(Order order);
    public Task<Order?> GetOrderById(string? id);
    public Task UpdateOrder(Order currentOrder, Order newOrder);
    public Task<IEnumerable<Order>> GetAllOrders();
}