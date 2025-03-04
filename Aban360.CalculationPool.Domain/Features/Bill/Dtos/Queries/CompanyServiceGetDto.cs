namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries
{
    public record CompanyServiceGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public short CompanyServiceTypeId { get; set; }
    }
}
