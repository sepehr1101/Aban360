using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Queries;

namespace Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Queries.Contracts
{
    public interface IRegionGetSingleHandler
    {
        Task<RegionGetDto> Handle(int id, CancellationToken cancellationToken);
    }
}
