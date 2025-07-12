namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record ServiceLinkCalculationDetailsDataOutputDto
    {
        public string ItemTitle { get; set; }
        public int ItemId { get; set; }
        public long Amount { get; set; }
        public long DiscountAmount { get; set; }

        public int InstallmentCount { get; set; }
        public long InstallmentAmount { get; set; }
        public int TypeId { get; set; }
    }
}
