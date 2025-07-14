namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record ReadingStatusStatementInputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public string FromReadingNumber { get; set; }
        public string ToReadingNumber { get; set; }

        public int ZoneId { get; set; }
        public bool IsRegisterDateJalali { get; set; }
    }
}
