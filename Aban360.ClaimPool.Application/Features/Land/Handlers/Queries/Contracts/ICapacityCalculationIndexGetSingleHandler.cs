using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts
{
    public interface ICapacityCalculationIndexGetSingleHandler
    {
        Task<CapacityCalculationIndexGetDto> Handle(short id, CancellationToken cancellationToken);
    }
}
