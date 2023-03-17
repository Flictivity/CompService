namespace CompService.Core.Models;

public class SparePart
{
    public string SparePartId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Article { get; set; } = null!;
    public SparePartCategory Category { get; set; } = null!;
    public int Count { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal RetailPrice { get; set; }
}