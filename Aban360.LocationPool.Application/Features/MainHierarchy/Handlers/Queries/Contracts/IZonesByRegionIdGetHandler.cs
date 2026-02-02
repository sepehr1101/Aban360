using Aban360.Common.BaseEntities;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts
{
    public interface IZonesByRegionIdGetHandler
    {
        Task<IEnumerable<NumericDictionary>> Handle(int id, CancellationToken cancellationToken);
    }
}
