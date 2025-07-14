namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record ReadingListInputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public string FromReadingNumber { get; set; }
        public string ToReadingNumber { get; set; }

        public ICollection<int> ZoneIds { get; set; }
    }
}
