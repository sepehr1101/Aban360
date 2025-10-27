namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Output
{
    public record Article11OutputDto
    {
        public short Id { get; set; }
        public long WaterMeterAmount { get; set; }
        public long WaterAmount { get; set; }
        public long? SewageMeterAmount { get; set; }
        public long? SewageAmount { get; set; }
        public bool IsDomestic { get; set; }
        public string? BlockCode { get; set; }
        public short ZoneId { get; set; }
        public string RegisterDateJalali { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public string? RemovedDateJalali { get; set; }
    }
}
