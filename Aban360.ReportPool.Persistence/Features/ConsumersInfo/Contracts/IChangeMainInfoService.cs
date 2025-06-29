using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts
{
    public interface IChangeMainInfoService
    {
        Task<Dictionary<string, List<string>>> GetInfo(string billId);
    }
}
