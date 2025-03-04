namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries
{
    public record CompanyServiceOfferingGetDto
    {
        public short Id { get; set; }
        public short CompanyServiceId { get; set; }
        public short OfferingId { get; set; }
    }
}
