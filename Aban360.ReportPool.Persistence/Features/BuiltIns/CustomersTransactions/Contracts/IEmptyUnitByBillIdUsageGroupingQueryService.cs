using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts
{
    public interface IEmptyUnitByBillIdUsageGroupingQueryService
    {
        Task<ReportOutput<EmptyUnitByBillIdSummaryHeaderOutputDto, EmptyUnitByBillIdUsageGroupingDataOutputDto>> Get(EmptyUnitInputDto input);
    }
}
