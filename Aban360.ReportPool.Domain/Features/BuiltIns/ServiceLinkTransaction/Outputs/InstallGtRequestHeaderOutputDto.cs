namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record InstallGtRequestHeaderOutputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public int Distance { get; set; }

        public string ReportDateJalali { get; set; }
        public int RecordCount { get; set; }
        public int CustomerCount { get; set; }
        public string? Title { get; set; }

        public int SumDistance { get; set; }

    }

}
