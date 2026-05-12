using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts
{
    public interface ILatestWaterMeterInfoQueryService
    {
        Task<LatestWaterMeterInfoDto> GetInfo(string billId);
        Task<string?> GetLatestChangeDateJalali(ZoneIdAndCustomerNumber input);
    }
}
