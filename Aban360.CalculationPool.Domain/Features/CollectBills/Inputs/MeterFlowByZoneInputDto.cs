namespace Aban360.CalculationPool.Domain.Features.CollectBills.Inputs
{
   public record MeterFlowByZoneInputDto
    {
        public int ZoneId { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
    }
}
