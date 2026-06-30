namespace Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto
{
    public record BillLatestListInputDto
    {
        public int ZoneId { get; set; }
        public string FromReadingNumber { get; set; }
        public string ToReadingNumber { get; set; }
        public BillLatestListInputDto(int zoneId, string fromReadingNumber, string toReadingNumber)
        {
            ZoneId = zoneId;
            FromReadingNumber = fromReadingNumber;
            ToReadingNumber = toReadingNumber;
        }
        public BillLatestListInputDto()
        {
        }
    }
}
