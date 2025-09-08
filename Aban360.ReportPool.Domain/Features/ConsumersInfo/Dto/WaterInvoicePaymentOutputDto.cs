namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record WaterInvoicePaymentOutputDto
    {
        public string PaymentDateJalali { get; set; }
        public string PaymentMethod { get; set; }
    }
}
