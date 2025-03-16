using Aban360.ClaimPool.Domain.Constants;

namespace Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands
{
    public record ChangeMeterReasonUpdateDto
    {
        public ChangeMeterReasonEnum Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
