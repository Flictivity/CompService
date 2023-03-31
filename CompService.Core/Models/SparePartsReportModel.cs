namespace CompService.Core.Models
{
    public class SparePartsReportModel
    {
        public List<SparePart>? SpareParts;
        public int TotalCount { get; set; }
        public double TotalPrice { get; set; }
        public double TotalCost { get; set; }
    }
}
