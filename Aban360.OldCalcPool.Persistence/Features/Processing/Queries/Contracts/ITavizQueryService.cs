using Aban360.Common.BaseEntities;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts
{
    public interface ITavizQueryService
    {
        Task<IEnumerable<MeterChangeInfoOutputDto>> Get(ZoneIdAndCustomerNumber input);
    }
}
