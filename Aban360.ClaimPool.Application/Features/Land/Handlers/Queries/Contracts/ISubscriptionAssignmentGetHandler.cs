using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts
{
    public interface ISubscriptionAssignmentGetHandler
    {
        Task<SubscriptionAssignmentGetDto> Handle(string input, CancellationToken cancellationToken);
    }
}
