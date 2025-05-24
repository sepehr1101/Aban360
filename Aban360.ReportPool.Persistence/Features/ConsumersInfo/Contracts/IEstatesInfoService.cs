using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts
{
    public interface IEstatesInfoService
    {
        Task<IEnumerable<EstatesInfoDto>> GetInfo(string billId);
    }
}
