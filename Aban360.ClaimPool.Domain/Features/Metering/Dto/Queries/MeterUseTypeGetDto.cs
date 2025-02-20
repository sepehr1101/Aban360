using Aban360.ClaimPool.Domain.Constants;

namespace Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries
{
    public record MeterUseTypeGetDto
    {
        public MeterUseTypeEnum Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
