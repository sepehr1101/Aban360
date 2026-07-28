using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Domain.Features.Tagging.Commands;

namespace Aban360.ReportPool.Persistence.Features.Tagging.Queries.Contracts
{
    public interface ITagService
    {
        Task<IEnumerable<TagDto>> GetAll();
        Task<TagDto?> GetById(int id);
    }
}
