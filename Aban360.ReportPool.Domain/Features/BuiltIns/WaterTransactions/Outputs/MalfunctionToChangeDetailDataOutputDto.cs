namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record MalfunctionToChangeDetailDataOutputDto
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string NationalId { get; set; }
        public string MobileNumber { get; set; }
        public string UsageTitle { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public string BillId { get; set; }
        public string ChangeDateJalali { get; set; }
        public string LatestMalfunctionDateJalali { get; set; }
        public string Duration { get; set; }

    }
}
