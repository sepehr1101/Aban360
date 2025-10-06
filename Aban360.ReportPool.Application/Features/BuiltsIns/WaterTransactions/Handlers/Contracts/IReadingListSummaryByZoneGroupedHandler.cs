using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts
{
    public interface IReadingListSummaryByZoneGroupedHandler
    {
        Task<ReportOutput<ReadingListHeaderOutputDto, ReportOutput<ReadingListSummaryDataOutputDto, ReadingListSummaryDataOutputDto>>> Handle(ReadingListInputDto input, CancellationToken cancellationToken);
        Task<ReportOutput<ReadingListHeaderOutputDto, ReadingListSummaryDataOutputDto>> HandleFlat(ReadingListInputDto input, CancellationToken cancellationToken);
    }
}
