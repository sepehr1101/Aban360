namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record ReadingInBetweenOutput
    {
        public int ZoneId { get; set; }
        public string ReadingNumber { get; set; } = default!;
        public int CustomerNumber { get; set; }
        public string BillId { get; set; } = default!;
    }
}
