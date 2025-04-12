using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;

namespace Aban360.ReportPool.Persistence.Features.Transactions.Contracts
{
    public interface ISubscriptionEventQueryService
    {
        Task<IEnumerable<EventsSummaryDto>> GetEventsSummaryDtos(string billId);
    }
}