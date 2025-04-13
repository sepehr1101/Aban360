using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;

namespace Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Implementations
{
    internal class LatestWaterMeterInfoGetHandler : ILatestWaterMeterInfoGetHandler
    {
        private readonly ILatestWaterMeterInfoQueryService _latestWaterMeterInfoQuery;
        public LatestWaterMeterInfoGetHandler(ILatestWaterMeterInfoQueryService latestWaterMeterInfoQuery)
        {
            _latestWaterMeterInfoQuery = latestWaterMeterInfoQuery;
            _latestWaterMeterInfoQuery.NotNull(nameof(latestWaterMeterInfoQuery));
        }

        public async Task<LatestWaterMeterInfoDto> Handle(string billId, CancellationToken cancellationToken)
        {
            var latestWaterMeterInfo = await _latestWaterMeterInfoQuery.GetInfo(billId);
            return latestWaterMeterInfo;
        }
    }
}
