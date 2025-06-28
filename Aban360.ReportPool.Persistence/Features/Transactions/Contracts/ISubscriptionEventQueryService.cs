using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Domain.Features.Transactions;

namespace Aban360.ReportPool.Persistence.Features.Transactions.Contracts
{
    public interface ISubscriptionEventQueryService
    {
        //Task<IEnumerable<EventsSummaryOutputDataDto>>
        Task<ReportOutput<EventsSummaryOutputHeaderDto, EventsSummaryOutputDataDto>> GetEventsSummaryDtos(string billId);
        Task<IEnumerable<EventsSummaryOutputDataDto>> GetBillDto(int zoneId, string registerDate, string fromReadingNumber, string toReadingNumber);
        Task<IEnumerable<BranchEventsDto>> GetBranchEventDtos(string billId);
        Task<IEnumerable<EventsSummaryOutputDataDto>> GetBillDto(int zoneId, string fromReadingNumber, string toReadingNumber);
    }
}