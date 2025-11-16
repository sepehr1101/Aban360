using Aban360.ReportPool.Domain.Features.Geo;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts
{
    public interface ILocationInfoService
    {
        Task<LocationInfoDto> GetInfo(string billId);
    }
}
