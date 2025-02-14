using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts
{
    public interface ISiphonGetSingleHandler
    {
        Task<SiphonGetDto> Handle(int id, CancellationToken cancellationToken);
    }
}
