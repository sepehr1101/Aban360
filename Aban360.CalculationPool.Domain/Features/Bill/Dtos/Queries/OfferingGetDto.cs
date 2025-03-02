using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries
{
    public record OfferingGetDto
    {
        public short Id { get; set; }
        public short OfferingUnitId { get; set; }
        public short OfferingGroupId { get; set; }
        public string Title { get; set; } = null!;
        public bool InstallmentOption { get; set; }
        public string? Description { get; set; }
    }
}