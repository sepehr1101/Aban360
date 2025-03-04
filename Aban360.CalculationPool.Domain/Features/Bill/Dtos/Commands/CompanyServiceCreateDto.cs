namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record CompanyServiceCreateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public short CompanyServiceTypeId { get; set; }
    }
}
