using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Implementations
{
    internal class LatestWaterMeterInfoQueryService : ILatestWaterMeterInfoQueryService
    {
        public Task<LatestWaterMeterInfoDto> GetInfo(string billId)
        {
            throw new NotImplementedException();
        }
    }
}
