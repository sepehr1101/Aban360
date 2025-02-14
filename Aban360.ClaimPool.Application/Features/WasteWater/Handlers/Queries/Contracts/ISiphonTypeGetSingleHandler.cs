using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts
{
    public interface ISiphonTypeGetSingleHandler
    {
        Task<SiphonTypeGetDto> Handle(short id, CancellationToken cancellationToken);
    }
}
