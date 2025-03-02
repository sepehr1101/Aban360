using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries
{
    public record OfferingGroupGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
