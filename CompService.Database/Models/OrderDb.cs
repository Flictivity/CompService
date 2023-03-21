using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CompService.Database.Models;

public class OrderDb
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string OrderId { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public ClientDb Client { get; set; } = null!;
    public DefectDb Defect { get; set; } = null!;
    public AppearanceDb Appearance { get; set; } = null!;
    public DeviceTypeDb DeviceType { get; set; } = null!;
    public BrandDb Brand { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string? DevicePassword { get; set; }
    public UserDb Operator { get; set; } = null!;
    public UserDb Master { get; set; } = null!;
    public int Status { get; set; }
    public List<SparePartDb>? SpareParts { get; set; }
    public List<FacilityDb>? Facilities { get; set; }
    public double Money { get; set; }
}