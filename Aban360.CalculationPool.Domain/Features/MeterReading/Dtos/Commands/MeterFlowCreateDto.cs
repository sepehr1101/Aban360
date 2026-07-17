using Aban360.CalculationPool.Domain.Constants;

namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record MeterFlowCreateDto
    {
        public MeterFlowStepEnum MeterFlowStepId { get; set; }
        public int FirstFlowId { get; set; }
        public string FileName { get; set; }
        public int ZoneId { get; set; }
        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }
        public int PrimaryCount { get; set; }
        public DateTime InsertDateTime { get; set; }
        public Guid InsertByUserId { get; set; }
        public DateTime? RemovedDateTime { get; set; }
        public Guid? RemovedByUserId { get; set; }
        public string? Description { get; set; }

    }
}