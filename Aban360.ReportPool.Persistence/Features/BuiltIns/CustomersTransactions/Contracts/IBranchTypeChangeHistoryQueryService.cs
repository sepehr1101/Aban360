using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts
{
    public interface IBranchTypeChangeHistoryQueryService
    {
        Task<ReportOutput<BranchTypeChangeHistoryHeaderOutputDto, BranchTypeChangeHistoryDataOutputDto>> GetInfo(BranchTypeChangeHistoryInputDto input);
    }
}
