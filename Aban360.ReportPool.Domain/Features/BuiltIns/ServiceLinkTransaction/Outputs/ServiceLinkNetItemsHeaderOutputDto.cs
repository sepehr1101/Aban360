namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record ServiceLinkNetItemsHeaderOutputDto
    {
        public string FromDataJalali { get; set; }
        public string ToDataJalali { get; set; }
        public string ReportDateJalali { get; set; }
        public int RecordCount { get; set; }

        public long SumAmount { get; set; }
        public long SumOffAmount { get; set; }
        public long SumFinalAmount { get; set; }
    }
}
