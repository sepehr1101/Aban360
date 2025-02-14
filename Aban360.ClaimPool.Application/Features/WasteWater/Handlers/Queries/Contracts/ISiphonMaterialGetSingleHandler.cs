using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts
{
    public interface ISiphonMaterialGetSingleHandler
    {
        Task<SiphonMaterialGetDto> Handle(short id, CancellationToken cancellationToken);
    }
}
