using Aban360.ClaimPool.Domain.Constants;

namespace Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands
{
    public record MeterUseTypeDeleteDto
    {
        public MeterUseTypeEnum Id { get; set; }
    }
}
