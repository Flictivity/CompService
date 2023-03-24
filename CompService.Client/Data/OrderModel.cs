
using CompService.Core.Enums;
using CompService.Core.Models;

namespace CompService.Client.Data;

public class OrderModel
{
    public Core.Models.Client? Client { get; set; } = null!;
    public User Operator { get; set; } = null!;
    public User Master { get; set; } = null!;
    public DevicePlace Place { get; set; } = null!;
    public RepairTypes RepairType { get; set; }
    public Defect Defect { get; set; } = null!;
    public Appearance Appearance { get; set; } = null!;
    public DeviceType DeviceType { get; set; } = null!;
    public Brand Brand { get; set; } = null!;
    public string? Model { get; set; }
    public string? Password { get; set; }
}