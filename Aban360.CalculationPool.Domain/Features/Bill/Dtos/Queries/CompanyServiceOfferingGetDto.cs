namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries
{
    public record CompanyServiceOfferingGetDto
    {
        public short Id { get; set; }
        public short CompanyServiceId { get; set; }
        public string CompanyServiceTitle { get; set; } = default!;
        public short OfferingId { get; set; }
        public string OfferingTitle { get; set; }= default!;
    }
}
