using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using System.Runtime.InteropServices;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts
{
    public interface INonPermanentBranchSummaryByZoneGroupedHandler
    {
        Task<ReportOutput<NonPermanentBranchHeaderOutputDto, ReportOutput<NonPermanentBranchSummaryByZoneGropedDataOutputDto, NonPermanentBranchSummaryByZoneGropedDataOutputDto>>> Handle(NonPermanentBranchByUsageAndZoneInputDto input, CancellationToken cancellationToken);
        Task<ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchSummaryByZoneGropedDataOutputDto>> HandleFlat(NonPermanentBranchByUsageAndZoneInputDto input, [Optional] CancellationToken cancellationToken);
    }
}
