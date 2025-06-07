namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs
{
    public record WaterPaymentReceivableInputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
    }
}
