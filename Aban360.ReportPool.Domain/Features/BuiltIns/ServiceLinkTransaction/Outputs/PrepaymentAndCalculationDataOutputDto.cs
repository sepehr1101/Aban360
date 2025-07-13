namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record PrepaymentAndCalculationDataOutputDto
    {
        public string ItemTitle { get; set; }
        public int  ItemId { get; set; }
        public long Amount { get; set; }

        public int Discount { get; set; }
        public int InstallmentNumber { get; set; }
        public int InstallmentCount { get; set; }


    }
}
