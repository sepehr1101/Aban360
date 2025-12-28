using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.Transactions;

namespace Aban360.ReportPool.Application.Features.Transactions.Handler.Contracts
{
    public interface ISubscriptionEventHandler
    {
        Task<ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto>> Handle(string input, string fromDate);
        Task<ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto>> HandleWithLastDb(string input, string fromDate);
    }
}
