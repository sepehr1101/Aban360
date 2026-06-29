using Aban360.CalculationPool.Domain.Constants;

namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record MeterReadingDetailExcludeInputDto
    {
        public int Id { get; set; }
        public ExcludedCauseEnum CauseId { get; set; }
    }
}
