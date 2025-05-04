using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;

namespace Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts
{
    public interface IIntervalBillPrerequisiteInfoHandler
    {
        Task<IntervalBillSubscriptionInfo> Handle(string billId, CancellationToken cancellationToken);
        Task<IEnumerable<IntervalBillSubscriptionInfo>> Handle(int zoneId, string registerDate, string fromReadingNumber, string toReadingNumber, CancellationToken cancellationToken);
        Task<IEnumerable<IntervalBillSubscriptionInfo>> Handle(int zoneId, string fromDate, string toDate, short usageId, short individualTypeId, short handover, string fromReadingNumber, string toReadingNumber);
    }
}