using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.Transactions;

namespace Aban360.ReportPool.Application.Features.Transactions.Handler.Contracts
{
    public interface ISubscriptionEventHandler
    {
        Task<long> ReconcileDebtAsync(string billId, Aban360.Common.ApplicationUser.IAppUser appUser, CancellationToken cancellationToken);
        Task<ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto>> Handle(SubscriptionCardexInput input, CancellationToken cancellationToken);
        Task<ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto>> HandleWithLastDb(SubscriptionCardexInput input,CancellationToken cancellationToken);
    }
}
