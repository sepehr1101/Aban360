namespace Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto
{
    public record BillHistoryInputDto
    {
        public string BillId { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
    }
}
