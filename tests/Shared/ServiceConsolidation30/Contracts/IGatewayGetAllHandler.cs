// Test-only contract for unchanged pre-refactor snapshots.
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts
{
    public interface IGatewayGetAllHandler
    {
        Task<ICollection<GatewayGetDto>> Handle(CancellationToken cancellationToken);
    }
}