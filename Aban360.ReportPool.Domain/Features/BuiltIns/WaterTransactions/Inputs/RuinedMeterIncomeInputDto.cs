namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record RuinedMeterIncomeInputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }

        public ICollection<int> ZoneIds { get; set; }
    }
}
