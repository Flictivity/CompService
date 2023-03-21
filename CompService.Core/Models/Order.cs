using CompService.Core.Enums;

namespace CompService.Core.Models;

public class Order
{
    public string OrderId { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public Client Client { get; set; } = null!;
    public Defect Defect { get; set; } = null!;
    public Appearance Appearance { get; set; } = null!;
    public DeviceType DeviceType { get; set; } = null!;
    public Brand Brand { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string? DevicePassword { get; set; }
    public User Operator { get; set; } = null!;
    public User Master { get; set; } = null!;
    public OrdersStatuses Status { get; set; }
    public List<SparePart>? SpareParts { get; set; }
    public List<Facility>? Facilities { get; set; }
    public double Money { get; set; }
}