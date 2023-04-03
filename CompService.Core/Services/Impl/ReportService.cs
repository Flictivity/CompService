using CompService.Core.Enums;
using CompService.Core.Models;

namespace CompService.Core.Services.Impl
{
    public class ReportService : IReportService
    {
        private readonly IOrderService _orderService;
        private readonly ISparePartService _sparePartService;

        public ReportService(IOrderService orderService, ISparePartService sparePartService)
        {
            _orderService = orderService;
            _sparePartService = sparePartService;
        }

        public async Task<IEnumerable<OrderReportModel>> GetOrdersReportForPeriodAsync(DateTime periodStart,
            DateTime periodEnd)
        {
            var orders = (await _orderService.GetAllOrdersAsync())
                .Where(x => x.OrderDate >= periodStart && x.OrderDate <= periodEnd && x.Status == OrdersStatuses.Closed)
                .OrderBy(x => x.OrderDate)
                .ToList();


            var res = (from order in orders
                       let facilitiesSum = order.Facilities?.Sum(x => x.Sum) ?? 0
                       let sparePartPriceSum = order.SpareParts?.Sum(x => x.Sum) ?? 0
                       let sparePartCostSum = order.SpareParts?.Sum(x => x.Item.PurchasePrice * x.ItemCount) ?? 0
                       select new OrderReportModel
                       {
                           OrderId = order.OrderId,
                           OrderDate = order.OrderDate,
                           FacilitiesSum = Math.Round(facilitiesSum, 2),
                           SparePartPriceSum = Math.Round(sparePartPriceSum, 2),
                           SparePartCostSum = Math.Round(sparePartCostSum, 2),
                           CostSum = Math.Round(facilitiesSum + sparePartCostSum),
                           PriceSum = Math.Round(facilitiesSum + sparePartPriceSum),
                           Profit = Math.Round(facilitiesSum + sparePartPriceSum - (facilitiesSum + sparePartCostSum))
                       }).ToList();

            return res;
        }

        public async Task<SparePartsReportModel> GetSparePartsReportAsync(SparePartCategory? category)
        {
            var spareParts = (await _sparePartService.GetAllSparePartsAsync()).ToList();

            List<SparePart> filtered;
            if (category is not null)
            {
                filtered = spareParts.Where(x => x.Category.CategoryId == category.CategoryId).ToList();
            }
            else
            {
                filtered = spareParts;
            }

            var reportModel = new SparePartsReportModel
            {
                SpareParts = filtered.ToList(),
                TotalCount = filtered.Sum(x => x.Count),
                TotalCost = Math.Round(filtered.Sum(x => (x.RetailPrice * x.Count))),
                TotalPrice = Math.Round(filtered.Sum(x => (x.PurchasePrice * x.Count)))
            };

            return reportModel;
        }
    }
}
