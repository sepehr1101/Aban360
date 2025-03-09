using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts
{
    public interface ISubscriptionTypeGetSingleHandler
    {
        Task<SubscriptionTypeGetDto> Handle(SubscriptionTypeEnum id, CancellationToken cancellationToken);
    }
}
