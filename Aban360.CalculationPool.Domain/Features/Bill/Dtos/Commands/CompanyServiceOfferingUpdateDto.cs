namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record CompanyServiceOfferingUpdateDto
    {
        public short Id { get; set; }
        public short CompanyServiceId { get; set; }
        public short OfferingId { get; set; }
    }
}
