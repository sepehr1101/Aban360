using Aban360.ClaimPool.Domain.Constants;

namespace Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries
{
    public record SubscriptionTypeGetDto
    {
        public SubscriptionTypeEnum Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
