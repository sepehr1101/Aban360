using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;

namespace Aban360.ReportPool.GatewayAdhoc.Features.ConsumersInfo.Contracts
{
    public interface IIntervalBillPrerequisiteInfoAddHoc
    {
        Task<IntervalBillSubscriptionInfo> Handle(string billId, CancellationToken cancellationToken);
        Task<IEnumerable<IntervalBillSubscriptionInfo>> Handle(int zoneId, string registerDate, string fromReadingNumber, string toReadingNumber, CancellationToken cancellationToken);
        Task<IEnumerable<IntervalBillSubscriptionInfo>> Handle(int zoneId, string fromDate, string toDate, short usageId, short individualTypeId, short handover, string fromReadingNumber, string toReadingNumber, CancellationToken cancellationToken;
    }
}