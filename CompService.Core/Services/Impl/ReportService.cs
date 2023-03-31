using CompService.Core.Models;

namespace CompService.Core.Services.Impl
{
    public class ReportService : IReportService
    {
        private readonly IOrderService _orderService;

        public ReportService(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IEnumerable<OrderReportModel>> GetOrdersReportForPeriodAsync(DateTime periodStart,
            DateTime periodEnd)
        {
            var orders = (await _orderService.GetAllOrdersAsync())
                .Where(x => x.OrderDate >= periodStart && x.OrderDate <= periodEnd)
                .OrderBy(x => x.OrderDate)
                .ToList();
            

            var res = (from order in orders
            let facilitiesSum = order.Facilities?.Sum(x => x.Sum) ?? 0
            let sparePartPriceSum = order.SpareParts?.Sum(x => x.Sum) ?? 0
            let sparePartCostSum = order.SpareParts?.Sum(x => x.Item.RetailPrice * x.ItemCount) ?? 0
            select new OrderReportModel
            {
                OrderId = order.OrderId,
                FacilitiesSum = Math.Round(facilitiesSum, 2),
                SparePartPriceSum = Math.Round(sparePartPriceSum, 2),
                SparePartCostSum = Math.Round(sparePartCostSum, 2),
                CostSum = Math.Round(facilitiesSum + sparePartCostSum),
                PriceSum = Math.Round(facilitiesSum + sparePartPriceSum),
                Profit = Math.Round(facilitiesSum + sparePartPriceSum - (facilitiesSum + sparePartCostSum))
            }).ToList();

            return res;
        }
    }
}
