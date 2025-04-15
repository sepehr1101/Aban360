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
    }
}