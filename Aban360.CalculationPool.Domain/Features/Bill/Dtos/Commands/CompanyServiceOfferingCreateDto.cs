namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record CompanyServiceOfferingCreateDto
    {
        public short Id { get; set; }
        public short CompanyServiceId { get; set; }
        public short OfferingId { get; set; }
    }
}
