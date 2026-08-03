using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts
{
    public interface IPendingPaymentsQueryService
    {
        Task<ReportOutput<PendingPaymentsHeaderOutputDto, PendingPaymentsDataOutputDto>> GetInfo(PendingPaymentsInputDto input);
        Task<ReportOutput<PendingPaymentsHeaderOutputDto, PendingPaymentSummaryDataOutputDto>> GetSummary(PendingPaymentsSummaryDto input);
        Task<ReportOutput<PendingPaymentsHeaderOutputDto, PendingPaymentSummaryByZoneAndUsageDataOutputDto>> GetSummary(PendingPaymentsInputDto input);

    }
}
