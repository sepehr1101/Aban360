namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record SewageWaterDistanceofRequestAndInstallationHeaderOutputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public string? FromReadingNumber{ get; set; }
        public string? ToReadingNumber { get; set; }
        public string ReportDateJalali { get; set; }
        public int RecordCount { get; set; }
        public string AverageDistance { get; set; }
        public float AverageDistanceNumber { get; set; }
        public string MaxDistance { get; set; }
        public string MinDistance { get; set; }
        public string Title { get; set; }

        public int CustomerCount { get; set; }
        public int SumDomesticUnit { get; set; }
        public int SumCommercialUnit { get; set; }
        public int SumOtherUnit { get; set; }
        public int TotalUnit { get; set; }
    }
}
