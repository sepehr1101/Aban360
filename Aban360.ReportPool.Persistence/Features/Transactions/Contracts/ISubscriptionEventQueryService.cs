using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Domain.Features.Transactions;

namespace Aban360.ReportPool.Persistence.Features.Transactions.Contracts
{
    public interface ISubscriptionEventQueryService
    {
        //Task<IEnumerable<EventsSummaryOutputDataDto>>
        Task<ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto>> GetEventsSummaryDtos(string billId, string fromDate);
        Task<IEnumerable<WaterEventsSummaryOutputDataDto>> GetBillDto(int zoneId, string registerDate, string fromReadingNumber, string toReadingNumber);
        Task<IEnumerable<BranchEventsDto>> GetBranchEventDtos(string billId);
        Task<IEnumerable<WaterEventsSummaryOutputDataDto>> GetBillDto(int zoneId, string fromReadingNumber, string toReadingNumber);
    }
}