namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record ZoneIdAndCustomerNumberGetDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
    }
}
