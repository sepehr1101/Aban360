namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Input
{
    public record Article11InputDto
    {
        public long WaterMeterAmount { get; set; }
        public long WaterAmount { get; set; }
        public long? SewageMeterAmount { get; set; }
        public long? SewageAmount { get; set; }
        public bool IsDomestic { get; set; }
        public string? BlockCode { get; set; }
        public int ZoneId { get; set; }
        public string? RegisterDateJalali { get; set; } 
        public string FromDateJalali { get; set; } 
        public string ToDateJalali { get; set; } 
        public string? RemovedDateJalali { get; set; }
    }
}
