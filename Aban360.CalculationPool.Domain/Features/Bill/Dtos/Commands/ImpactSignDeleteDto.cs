using Aban360.CalculationPool.Domain.Constants;

namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record ImpactSignDeleteDto
    {
        public ImpactSignEnum Id { get; set; }
    }
}
