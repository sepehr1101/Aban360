using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Domain.Features.Tagging.Commands;

namespace Aban360.ReportPool.Persistence.Features.Tagging.Queries.Contracts
{
    public interface IMainTagGroupService
    {
        Task Insert(MainTagGroupInsertDto input);
        Task Update(MainTagGroupUpdateDto input);
        Task Remove(MainTagGroupRemoveDto input);
        Task<IEnumerable<MainTagGroupGetDto>> GetValid();
        Task<MainTagGroupGetDto> GetValid(int id);

    }
}
