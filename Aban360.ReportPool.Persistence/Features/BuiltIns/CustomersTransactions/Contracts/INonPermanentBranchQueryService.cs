using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts
{
    public interface INonPermanentBranchQueryService
    {
        Task<ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchDataOutputDto>> GetInfo(NonPermanentBranchInputDto input);
    }
}
