using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Queries.Contracts
{
    public interface IMeterChangeGetHandler
    {
        Task<IEnumerable<MeterChangeInfoOutputDto>> Handle(string billId, CancellationToken cancellationToken);
    }
}
