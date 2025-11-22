namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record LatesTavizInfo
    {
        public string? TavizDateJalali { get; set; }
        public string? TavizCause { get; set; }
        public string? TavizRegisterDateJalali { get; set; }
        public int? TavizNumber { get; set; }
    }
}
