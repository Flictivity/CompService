﻿
using CompService.Core.Enums;
using CompService.Core.Models;

namespace CompService.Client.Data;

public class OrderModel
{
    public string? OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public Core.Models.Client? Client { get; set; } 
    public User? Operator { get; set; }
    public User? Master { get; set; }
    public DevicePlace? Place { get; set; }
    public RepairTypes RepairType { get; set; }
    public Defect? Defect { get; set; }
    public Appearance? Appearance { get; set; }
    public DeviceType? DeviceType { get; set; }
    public Brand? Brand { get; set; }
    public string? Model { get; set; }
    public string? Password { get; set; }
    public List<OrderListModel<SparePart>>? SpareParts { get; set; }
    public List<OrderListModel<Facility>>? Facilities { get; set; }
    public double Money { get; set; }
    public OrdersStatuses Status { get; set; }
}