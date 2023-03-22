using CompService.Core.Enums;

namespace CompService.Core.Models;

public class Order
{
    public string OrderId { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public string ClientId { get; set; } = null!;
    public string DefectId { get; set; } = null!;
    public string AppearanceId { get; set; } = null!;
    public string DeviceTypeId { get; set; } = null!;
    public string BrandId { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string? DevicePassword { get; set; }
    public string OperatorId { get; set; } = null!;
    public string MasterId { get; set; } = null!;
    public OrdersStatuses Status { get; set; }
    public RepairTypes RepairType { get; set; }
    public List<OrderListModel<SparePart>>? SpareParts { get; set; }
    public List<OrderListModel<Facility>>? Facilities { get; set; }
    public double Money { get; set; }
    public string DevicePlaceId { get; set; } = null!;
}