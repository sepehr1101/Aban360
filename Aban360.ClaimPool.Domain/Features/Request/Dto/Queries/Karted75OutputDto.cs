namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record Karted75OutputDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string ReadingNumber { get; set; }
        public long Amount { get; set; }
        public string RegisterDateJalali { get; set; }
        public int UsageId { get; set; }
        public int MeterDiameterId { get; set; }
    }
}
