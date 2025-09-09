using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts
{
    public interface INerkhGetAllHandler
    {
        Task<IEnumerable<NerkhGetDto>> Handle(int nerkh, CancellationToken cancellationToken);
        Task<IEnumerable<NerkhGetDto>> Handle( CancellationToken cancellationToken);
    }
}
