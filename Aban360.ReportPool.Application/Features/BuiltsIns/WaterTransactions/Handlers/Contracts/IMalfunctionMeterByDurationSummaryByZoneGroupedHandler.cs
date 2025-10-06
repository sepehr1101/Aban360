using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts
{
    public interface IMalfunctionMeterByDurationSummaryByZoneGroupedHandler
    {
        Task<ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, ReportOutput<MalfunctionMeterByDurationSummaryDataOutputDto, MalfunctionMeterByDurationSummaryDataOutputDto>>> Handle(MalfunctionMeterByDurationInputDto input, CancellationToken cancellationToken);
        Task<ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationSummaryDataOutputDto>> HandleFlat(MalfunctionMeterByDurationInputDto input, CancellationToken cancellationToken);
    }
}
