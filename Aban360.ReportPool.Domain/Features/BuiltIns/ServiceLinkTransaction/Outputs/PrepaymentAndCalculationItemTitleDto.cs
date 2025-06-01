namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record PrepaymentAndCalculationItemTitleDto
    {
        public string PaymentDetail { get; set; }
        public string Prepayment { get; set; }
    }
}
