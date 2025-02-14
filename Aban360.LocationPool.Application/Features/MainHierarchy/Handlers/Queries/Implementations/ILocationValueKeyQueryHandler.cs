using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries.ValueKeys;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Implementations
{
    public interface ILocationValueKeyQueryHandler
    {
        Task<LocationTree> Handle(CancellationToken cancellationToken);
        Task<LocationTree> Handle(ICollection<int> selectedZoneIds, CancellationToken cancellationToken);
    }
}