using CompService.Core.Models;
using CompService.Core.Results;

namespace CompService.Core.Repositories;

public interface IOrderRepository
{
    public Task Create(Order order);
    public Task<Order?> GetOrderById(string id);
    public Task<IEnumerable<OrderListModel<SparePart>>> GetOrderSpareParts(string id);
    public Task<IEnumerable<OrderListModel<Facility>>> GetOrderFacilities(string id);
    public Task<int> GetMasterOrdersCount(string masterId);
    public Task UpdateOrder(Order currentOrder, Order newOrder);
    public Task<IEnumerable<Order>> GetAllOrders();

    public Task<ListDataResult<OrderTableModel>> GetAllOrdersForTable(int itemCount, int pageNum, User user,
        string searchText, string field, bool desc = false);
}