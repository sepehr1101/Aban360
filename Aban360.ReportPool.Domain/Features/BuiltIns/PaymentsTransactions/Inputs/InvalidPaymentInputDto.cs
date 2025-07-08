namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs
{
    public record InvalidPaymentInputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
    }
}
