using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts
{
    public interface IRemovedBillSummaryByZoneQueryService
    {
        Task<ReportOutput<RemovedBillHeaderOutputDto, RemovedBillSummaryDataOutputDto>> GetInfo(RemovedBillInputDto input);
    }
}
