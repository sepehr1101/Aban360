using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;

namespace Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Implementations
{
    internal sealed class IntervalBillPrerequisiteInfoHandler : IIntervalBillPrerequisiteInfoHandler
    {
        private readonly IIntervalBillPrerequisiteInfoService _intervalBillPrerequisiteInfoService;
        public IntervalBillPrerequisiteInfoHandler(IIntervalBillPrerequisiteInfoService intervalBillPrerequisiteInfoService)
        {
            _intervalBillPrerequisiteInfoService = intervalBillPrerequisiteInfoService;
            _intervalBillPrerequisiteInfoService.NotNull(nameof(intervalBillPrerequisiteInfoService));
        }
        public async Task<IntervalBillSubscriptionInfo> Handle(string billId, CancellationToken cancellationToken)
        {
            var info = await _intervalBillPrerequisiteInfoService.GetInfo(billId);
            return info;
        }
        public async Task<IEnumerable<IntervalBillSubscriptionInfo>> Handle(int zoneId, string registerDate, string fromReadingNumber, string toReadingNumber, CancellationToken cancellationToken)
        {
            IEnumerable<IntervalBillSubscriptionInfo> info = await _intervalBillPrerequisiteInfoService.GetInfo(zoneId, registerDate, fromReadingNumber, toReadingNumber);
            return info;
        }
        public async Task<IEnumerable<IntervalBillSubscriptionInfo>> Handle(int zoneId, string fromDate, string toDate, short usageId, short individualTypeId, short handover, string fromReadingNumber, string toReadingNumber)
        {
            IEnumerable<IntervalBillSubscriptionInfo> info=await _intervalBillPrerequisiteInfoService.GetInfo(zoneId, fromDate, toDate, usageId,individualTypeId, handover, fromReadingNumber, toReadingNumber);
            return info;
        }
    }
}