namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record WaterModifiedBillsInputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public ICollection<int> TypeIds { get; set; }
        public ICollection<int> ZoneIds { get; set; }
    }
}
