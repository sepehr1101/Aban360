namespace Aban360.ReportPool.Domain.Features.Geo
{
    public record LocationBase64Dto
    {
        public int CustomerNumber { get; set; }
        public int ZoneId { get; set; }
        public string BillId { get; set; }
        public string? Base64 { get; set; }
        public LocationBase64Dto(int customerNumber, int zoneId, string billId, string? base64)
        {
            CustomerNumber = customerNumber;
            ZoneId = zoneId;
            BillId = billId;
            Base64 = base64;
        }
        public LocationBase64Dto()
        {
        }
    }
}
