using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts
{
    public interface IRuinedMeterIncomeSummaryByZoneGroupedHandler
    {
        Task<ReportOutput<RuinedMeterIncomeHeaderOutputDto, ReportOutput<RuinedMeterIncomeSummaryDataOutputDto, RuinedMeterIncomeSummaryDataOutputDto>>> Handle(RuinedMeterIncomeInputDto input, CancellationToken cancellationToken);
        Task<ReportOutput<RuinedMeterIncomeHeaderOutputDto, RuinedMeterIncomeSummaryDataOutputDto>> HandleFlat(RuinedMeterIncomeInputDto input, CancellationToken cancellationToken);
    }
}
