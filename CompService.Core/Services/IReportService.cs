using CompService.Core.Models;

namespace CompService.Core.Services
{
    public interface IReportService
    {
        public Task<IEnumerable<OrderReportModel>> GetOrdersReportForPeriodAsync(DateTime periodStart,
            DateTime periodEnd);
        public Task<SparePartsReportModel> GetSparePartsReportAsync(SparePartCategory? category);
    }
}
