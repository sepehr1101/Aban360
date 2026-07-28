using Aban360.ReportPool.Domain.Features.Tagging;

namespace Aban360.ReportPool.Persistence.Features.Tagging.Queries.Contracts
{
    public interface IMainTagGroupService
    {
        Task<IEnumerable<MainTagGroupGetDto>> GetValid();
        Task<MainTagGroupGetDto> GetValid(int id);
    }
}
