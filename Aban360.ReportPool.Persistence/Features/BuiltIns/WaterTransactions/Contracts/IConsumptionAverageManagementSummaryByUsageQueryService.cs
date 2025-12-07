using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts
{
    public interface IConsumptionAverageManagementSummaryByUsageQueryService
    {
        Task<ReportOutput<ConsumptionAverageManagementHeaderOutputDto, ConsumptionAverageManagementSummaryDataOutputDto>> Get(ConsumptionAverageManagementInputDto input, string groupField);
        Task<IEnumerable<ConsumptionAverageManagementSummaryOutputDto>> Get(ConsumptionAverageManagementSummrayInputDto input);
    }
}
