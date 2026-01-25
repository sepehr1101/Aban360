using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts
{
    public interface ISubscriptionQueryService
    {
        Task<SubscriptionAssignmentGetDto> Get(string input);
        Task<SubscriptionGetDto> GetInfo(string input);
    }
}
