namespace Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto
{
    public record BillLatestListInputDto
    {
        public int ZoneId { get; set; }
        public string FromReadingNumber { get; set; }
        public string ToReadingNumber { get; set; }
    }
}
