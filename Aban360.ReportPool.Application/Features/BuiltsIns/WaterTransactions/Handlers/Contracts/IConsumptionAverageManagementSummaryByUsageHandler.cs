using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using static Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Implementations.ConsumptionAverageManagementSummaryByUsageHandler;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts
{
    public interface IConsumptionAverageManagementSummaryByUsageHandler
    {
        Task<ReportOutput<ConsumptionAverageManagementHeaderOutputDto, ConsumptionAverageManagementSummaryDataOutputDto>> Handle(ConsumptionAverageManagementInputDto input, CancellationToken cancellationToken);
        Task<ReportOutput<QuarterDto, KeyValueDto>> HandleSummary(ConsumptionAverageManagementSummrayInputDto input, CancellationToken cancellationToken);
    }
}
