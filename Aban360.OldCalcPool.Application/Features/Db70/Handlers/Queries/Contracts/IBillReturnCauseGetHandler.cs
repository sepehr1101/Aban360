using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;

namespace Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Contracts
{
    public interface IBillReturnCauseGetHandler
    {
        Task<BillReturnCauseGetDto> Handle(SearchShortInputDto input, CancellationToken cancellationToken);
    }
}
