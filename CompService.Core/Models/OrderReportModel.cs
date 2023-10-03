namespace CompService.Core.Models;

public class OrderReportModel
{
    public string OrderId { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public double FacilitiesSum { get; set; }
    public double SparePartPriceSum { get; set; }
    public double SparePartCostSum { get; set; }
    public double CostSum { get; set; }
    public double PriceSum { get; set; }
    public double Profit { get; set; }
}