using CompService.Core.Models;

namespace CompService.Client.Data;

public class SparePartModel
{
    public string SparePartId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Article { get; set; } = string.Empty;
    public SparePartCategory? Category { get; set; } = null!;
    public int Count { get; set; } = 0;
    public double PurchasePrice { get; set; } = 0;
    public double RetailPrice { get; set; } = 0;
}