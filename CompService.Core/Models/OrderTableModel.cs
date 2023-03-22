namespace CompService.Core.Models;

public class OrderTableModel
{
    public string OrderId { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public string? Status { get; set; }
    public string ClientSurname { get; set; } = null!;
    public string ClientPhoneNumber { get; set; } = null!;
    public string Defect { get; set; } = null!;
    public string OperatorName { get; set; } = null!;
    public string MasterName { get; set; } = null!;
    public string Place { get; set; } = null!;
    public double Sum { get; set; }
}