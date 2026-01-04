using Aban360.Common.BaseEntities;

namespace Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Contracts
{
    public interface IOfferingGetAllHandler
    {
        Task<IEnumerable<NumericDictionary>> Handle(CancellationToken cancellationToken);
    }
}
