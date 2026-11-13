// Test-only contract retained for pre-refactor snapshots.
using Aban360.Common.BaseEntities;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts
{
    public interface IRegionDictionaryGetAllHandler
    {
        Task<IEnumerable<NumericDictionary>> Handle(CancellationToken cancellationToken);
    }
}
