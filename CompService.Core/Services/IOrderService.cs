using CompService.Core.Models;
using CompService.Core.Results;

namespace CompService.Core.Services;

public interface IOrderService
{
    public Task<BaseResult> Create(Order order);
    public Task<Order?> GetOrderById(string? id);
    public Task<BaseResult> UpdateOrder(Order currentOrder, Order newOrder);
    public Task<IEnumerable<Order>> GetAllOrders();
}