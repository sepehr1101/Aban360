using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts
{
    public interface ITable1GetAllHandler
    {
        Task<IEnumerable<Table1GetDto>> Handle( CancellationToken cancellationToken);
    }
}
