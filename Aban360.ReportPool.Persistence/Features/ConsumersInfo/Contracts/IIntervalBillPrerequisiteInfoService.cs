using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts
{
    public interface IIntervalBillPrerequisiteInfoService
    {
        Task<IntervalBillSubscriptionInfo> GetInfo(string billId);
        Task<IEnumerable<IntervalBillSubscriptionInfo>> GetInfo(int zoneId, string registerDate, string fromReadingNumber, string toReadingNumber);
        Task<IEnumerable<IntervalBillSubscriptionInfo>> GetInfo(int zoneId, string fromDate, string toDate, short usageId, short individualTypeId, short handover, string fromReadingNumber, string toReadingNumber);
    }
}