using Aban360.ClaimPool.Domain.Constants;

namespace Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands
{
    public record ChangeMeterReasonDeleteDto
    {
        public ChangeMeterReasonEnum Id { get; set; }
    }
}
