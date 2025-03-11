using Aban360.CalculationPool.Domain.Constants;

namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record ImpactSignUpdateDto
    {
        public ImpactSignEnum Id { get; set; }
        public string Title { get; set; } = null!;
        public short Multiplier { get; set; }
        public string? Description { get; set; }
    }
}
