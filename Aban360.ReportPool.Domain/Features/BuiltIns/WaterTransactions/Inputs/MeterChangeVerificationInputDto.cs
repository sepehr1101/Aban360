namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record MeterChangeVerificationInputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public bool IsChangeDate { get; set; }
        public IEnumerable<int> ZoneIds { get; set; }
    }
}
