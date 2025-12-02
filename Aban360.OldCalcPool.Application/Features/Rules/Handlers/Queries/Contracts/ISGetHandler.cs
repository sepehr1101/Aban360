using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts
{
    public interface ISGetHandler
    {
        Task<IEnumerable<SGetDto>> Handle(string fromDateJalali, string toDateJalali, CancellationToken cancellationToken);

    }
}
