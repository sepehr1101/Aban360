namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries
{
    public record CompanyServiceGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public bool IsMultiSelect { get; set; } = false;
        public short CompanyServiceTypeId { get; set; }
        public string CompanyServiceTypeTitle { get; set; }
    }
}
