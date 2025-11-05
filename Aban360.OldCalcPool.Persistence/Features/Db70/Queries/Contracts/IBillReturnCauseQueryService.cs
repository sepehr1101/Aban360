using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;

namespace Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts
{
    public interface IBillReturnCauseQueryService
    {
        Task<BillReturnCauseGetDto> Get(SearchShortInputDto input);
        Task<IEnumerable<BillReturnCauseGetDto>> Get();
    }
}
