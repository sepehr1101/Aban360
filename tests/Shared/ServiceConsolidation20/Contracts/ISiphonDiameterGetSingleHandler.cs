// Test-only contract retained for pre-refactor snapshots.
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts
{
    public interface ISiphonDiameterGetSingleHandler
    {
        Task<SiphonDiameterGetDto> Handle(short id, CancellationToken cancellationToken);
    }
}
