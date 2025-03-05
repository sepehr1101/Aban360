using Aban360.CalculationPool.Domain.Constants;

namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record CompanyServiceTypeUpdateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public TariffCalculationModeEnum TariffCalculationModeId { get; set; }
        public string? Description { get; set; }
    }
}
