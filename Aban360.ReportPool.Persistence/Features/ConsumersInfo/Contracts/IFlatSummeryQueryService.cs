using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;

namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    public interface IFlatSummeryQueryService
    {
        Task<IEnumerable<ResultFlatDto>> GetInfo(string billId);
    }
}
