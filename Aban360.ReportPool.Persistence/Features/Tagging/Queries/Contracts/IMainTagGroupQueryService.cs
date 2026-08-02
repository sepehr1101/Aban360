using Aban360.ReportPool.Domain.Features.Tagging;

namespace Aban360.ReportPool.Persistence.Features.Tagging.Queries.Contracts
{
    public interface IMainTagGroupQueryService
    {
        Task<IEnumerable<MainTagGroupGetDto>> GetValid();
        Task<MainTagGroupGetDto> GetValid(int id);
    }
}
