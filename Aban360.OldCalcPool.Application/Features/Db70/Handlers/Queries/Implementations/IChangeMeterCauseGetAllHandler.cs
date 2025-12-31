using Aban360.Common.BaseEntities;

namespace Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Implementations
{
    public interface IChangeMeterCauseGetAllHandler
    {
        Task<IEnumerable<NumericDictionary>> Handle(CancellationToken cancellationToken);
    }
}
