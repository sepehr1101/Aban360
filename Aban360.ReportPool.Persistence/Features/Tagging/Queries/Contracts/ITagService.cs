using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Domain.Features.Tagging.Commands;

namespace Aban360.ReportPool.Persistence.Features.Tagging.Queries.Contracts
{
    public interface ITagService
    {
        Task<int> Create(CreateTagDto dto);
        Task<bool> Delete(int id);
        Task<IEnumerable<TagDto>> GetAll();
        Task<TagDto?> GetById(int id);
        Task<bool> Update(UpdateTagDto dto);
    }
}
