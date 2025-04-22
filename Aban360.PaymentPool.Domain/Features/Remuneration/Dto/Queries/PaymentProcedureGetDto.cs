using Aban360.PaymentPool.Domain.Constansts;

namespace Aban360.PaymentPool.Domain.Features.Remuneration.Dto.Queries
{
    public record PaymentProcedureGetDto
    {
        public PaymentProcedureEnum Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Icon { get; set; }
        public string? Description { get; set; }
    }
}
