using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts
{
    public interface INerkhGetHandler
    {
        Task<IEnumerable<NerkhGetDto>> Handle(int id, int nerkh, CancellationToken cancellationToken);
    }
}
