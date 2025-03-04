namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record CompanyServiceTypeCreateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
    }
}
