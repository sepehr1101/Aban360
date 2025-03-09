using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts
{
    public interface ISubscriptionTypeGetAllHandler
    {
        Task<ICollection<SubscriptionTypeGetDto>> Handle(CancellationToken cancellationToken);
    }
}
