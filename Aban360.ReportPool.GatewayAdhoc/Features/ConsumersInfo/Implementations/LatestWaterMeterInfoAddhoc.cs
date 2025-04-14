using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.GatewayAdhoc.Features.ConsumersInfo.Contracts;

namespace Aban360.ReportPool.GatewayAdhoc.Features.ConsumersInfo.Implementations
{
    internal class LatestWaterMeterInfoAddhoc : ILatestWaterMeterInfoAddhoc
    {
        private readonly ILatestWaterMeterInfoGetHandler _latestWaterMeterInfoHandler;
        public LatestWaterMeterInfoAddhoc(ILatestWaterMeterInfoGetHandler latestWaterMeterInfoHandler)
        {
            _latestWaterMeterInfoHandler = latestWaterMeterInfoHandler;
            _latestWaterMeterInfoHandler.NotNull(nameof(latestWaterMeterInfoHandler));
        }

        public async Task<LatestWaterMeterInfoDto> Handle(string billId, CancellationToken cancellationToken)
        {
            var waterMeterInfo = await _latestWaterMeterInfoHandler.Handle(billId, cancellationToken);
            return waterMeterInfo;
        }
    }
}
