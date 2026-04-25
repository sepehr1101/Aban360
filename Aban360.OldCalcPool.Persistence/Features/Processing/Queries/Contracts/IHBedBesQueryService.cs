using Aban360.Common.BaseEntities;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts
{
    public interface IHBedBesQueryService
    {
        Task<bool> Get(ZoneIdAndCustomerNumber inputDto);
    }
}
