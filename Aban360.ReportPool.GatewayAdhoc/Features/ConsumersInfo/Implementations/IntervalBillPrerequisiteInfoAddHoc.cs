using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.GatewayAdhoc.Features.ConsumersInfo.Contracts;

namespace Aban360.ReportPool.GatewayAdhoc.Features.ConsumersInfo.Implementations
{
    internal sealed class IntervalBillPrerequisiteInfoAddHoc : IIntervalBillPrerequisiteInfoAddHoc
    {
        private readonly IIntervalBillPrerequisiteInfoHandler _intervalBillPrerequisiteInfoHandler;
        public IntervalBillPrerequisiteInfoAddHoc(IIntervalBillPrerequisiteInfoHandler intervalBillPrerequisiteInfoHandler)
        {
            _intervalBillPrerequisiteInfoHandler = intervalBillPrerequisiteInfoHandler;
            _intervalBillPrerequisiteInfoHandler.NotNull(nameof(intervalBillPrerequisiteInfoHandler));
        }
        public async Task<IntervalBillSubscriptionInfo> Handle(string billId, CancellationToken cancellationToken)
        {
            return await _intervalBillPrerequisiteInfoHandler.Handle(billId, cancellationToken);
        }
        public async Task<IEnumerable<IntervalBillSubscriptionInfo>> Handle(int zoneId, string registerDate, string fromReadingNumber, string toReadingNumber, CancellationToken cancellationToken)
        {
            return await _intervalBillPrerequisiteInfoHandler.Handle(zoneId, registerDate, fromReadingNumber, toReadingNumber, cancellationToken);
        }
    }
}