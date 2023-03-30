using CompService.Core.Models;
using CompService.Core.Results;

namespace CompService.Core.Services;

public interface IOrderService
{
    public Task<BaseResult> CreateAsync(Order order);
    public Task<Order?> GetOrderByIdAsync(string id);
    public Task<int> GetMasterOrdersCount(string masterId);
    public Task<BaseResult> UpdateOrderAsync(Order currentOrder, Order newOrder);
    public Task<IEnumerable<Order>> GetAllOrdersAsync();

    public Task<ListDataResult<OrderTableModel>> GetAllOrdersForTableAsync(int itemCount, int pageNum, User user,
        string searchText = "", string field = "OrderDate", bool desc = false);
}