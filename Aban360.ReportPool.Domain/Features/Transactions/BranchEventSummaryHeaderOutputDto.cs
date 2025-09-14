namespace Aban360.ReportPool.Domain.Features.Transactions
{
    public record BranchEventSummaryHeaderOutputDto
    {
        public string Title { get; set; }
        public string ReportDateJalali { get; set; }

        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string ZoneTitle { get; set; }
        public string RegionTitle { get; set; }
        public string BillId { get; set; }
        public string ReadingNumber { get; set; }

        public string UsageTitle { get; set; }
        public string JobTitle { get; set; }
        public string GuildTitle { get; set; }

        public int DomesticeUnit { get; set; }
        public int CommercialUnit { get; set; }
        public int OtherUnit { get; set; }
        public string MeterDiameterTitle { get; set; }
        public string SiphonDiameterTitle { get; set; }

        public long Remained { get; set; }
    }
}
