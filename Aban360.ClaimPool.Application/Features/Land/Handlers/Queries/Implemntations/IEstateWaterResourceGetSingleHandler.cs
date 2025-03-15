using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    public interface IEstateWaterResourceGetSingleHandler
    {
        Task<EstateWaterResourceGetDto> Handle(short id, CancellationToken cancellationToken);
    }
}
