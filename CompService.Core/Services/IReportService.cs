using CompService.Core.Models;
using CompService.Core.Results;

namespace CompService.Core.Services
{
    public interface IReportService
    {
        public Task<IEnumerable<OrderReportModel>> GetOrdersReportForPeriodAsync(DateTime periodStart,
            DateTime periodEnd);
    }
}
