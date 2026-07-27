using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Domain.Features.Tagging.Commands;

namespace Aban360.ReportPool.Persistence.Features.Tagging.Queries.Contracts
{
    public interface ITagGroupService
    {
        Task<int> Create(CreateTagGroupDto dto);
        Task<bool> Delete(int id);
        Task<IEnumerable<TagGroupDto>> GetAll();
        Task<TagGroupDto?> GetById(int id);
        Task<TagGroupDto?> GetByStringCode(string input);
        Task<bool> Update(UpdateTagGroupDto dto);
    }
}
