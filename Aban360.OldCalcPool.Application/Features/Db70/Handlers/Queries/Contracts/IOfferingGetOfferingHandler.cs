using Aban360.Common.BaseEntities;

namespace Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Contracts
{
    public interface IOfferingGetOfferingHandler
    {
        Task<NumericDictionary> Handle(SearchByIdInput input, CancellationToken cancellationToken);
    }
}
