using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Queries.Contracts
{
    public interface IMeterComparisonBatchGetHandler
    {
        Task<DetailSummary<MeterComparisonBatchHeaderOutputDto, MeterComparisonBatchDataOutputDto>> Handle(MeterComparisonBatchInputDto input, CancellationToken cancellationToken);
    }
}
