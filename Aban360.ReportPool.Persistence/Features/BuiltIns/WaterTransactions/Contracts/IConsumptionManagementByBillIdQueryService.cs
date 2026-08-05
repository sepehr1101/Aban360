using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts
{
    public interface IConsumptionManagementByBillIdQueryService
    {
        Task<IEnumerable<ConsumptionManagementByBillIdGetDto>> Get(ConsumptionManagementByBillIdDto inputDto);
    }
}
