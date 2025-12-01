using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts
{
    public interface IConsumptionAverageManagementQueryService
    {
        Task<ReportOutput<ConsumptionAverageManagementHeaderOutputDto, ConsumptionAverageManagementDataOutputDto>> Get(ConsumptionAverageManagementInputDto input);
    }
}
