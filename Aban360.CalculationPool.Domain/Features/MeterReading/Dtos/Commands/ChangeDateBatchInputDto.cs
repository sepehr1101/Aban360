namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record ChangeDateBatchInputDto
    {
        public int MeterFlowId { get; set; }
        public string FromReadingNumber { get; set; }
        public string ToReadingNumber { get; set; }
        public string DateJalali { get; set; }
        public bool IsConfirm { get; set; }
    }
}
