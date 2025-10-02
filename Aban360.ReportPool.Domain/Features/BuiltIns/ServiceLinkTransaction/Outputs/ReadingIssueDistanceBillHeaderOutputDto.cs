namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record ReadingIssueDistanceBillHeaderOutputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }

        public string ReportDateJalali { get; set; }
        public int RecordCount { get; set; }

        public int CustomerCount { get; set; }
        public float SumDomesticUnit { get; set; }
        public float SumCommercialUnit { get; set; }
        public float SumOtherUnit { get; set; }
        public float TotalUnit { get; set; }

    }
}
