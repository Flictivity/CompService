namespace CompService.Database.Models;

public class SparePartDb
{
    public string SparePartId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Article { get; set; } = null!;
    public SparePartCategoryDb Category { get; set; } = null!;
    public int Count { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal RetailPrice { get; set; }
}