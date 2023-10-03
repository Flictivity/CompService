using CompService.Core.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CompService.Database.Models;

public class OrderDb
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
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
    public int Status { get; set; }
    public int RepairType { get; set; }
    public List<OrderListModel<string>>? SpareParts { get; set; }
    public List<OrderListModel<string>>? Facilities { get; set; }
    public double Money { get; set; }
    public string DevicePlaceId { get; set; } = null!;
}