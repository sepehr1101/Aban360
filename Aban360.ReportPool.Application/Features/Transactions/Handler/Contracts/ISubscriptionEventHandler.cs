using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.Transactions;

namespace Aban360.ReportPool.Application.Features.Transactions.Handler.Contracts
{
    public interface ISubscriptionEventHandler
    {
        Task<ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto>> Handle(CardexInput input, CancellationToken cancellationToken);
        Task<ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto>> HandleWithLastDb(CardexInput input,CancellationToken cancellationToken);
    }
}
