namespace CompService.Core.Models;

public class DevicePlace
{
    public string PlaceId { get; set; } = null!;
    public string Info { get; set; } = null!;
    public Order Order { get; set; } = null!;
    public bool IsOccupied { get; set; }
}