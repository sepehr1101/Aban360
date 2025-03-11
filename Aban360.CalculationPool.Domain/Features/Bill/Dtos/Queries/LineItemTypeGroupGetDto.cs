using Aban360.CalculationPool.Domain.Constants;

namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries
{
    public record LineItemTypeGroupGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public ImpactSignEnum ImpactSignId { get; set; }
        public string ImpactSignTitle{ get; set; }
        public string? Description { get; set; }
    }
}
