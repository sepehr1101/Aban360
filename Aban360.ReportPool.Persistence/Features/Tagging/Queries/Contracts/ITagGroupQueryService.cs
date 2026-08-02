using Aban360.ReportPool.Domain.Features.Tagging;

namespace Aban360.ReportPool.Persistence.Features.Tagging.Queries.Contracts
{
    public interface ITagGroupQueryService
    {
        Task<IEnumerable<TagGroupDto>> GetAll();
        Task<TagGroupDto?> GetById(int id);
        Task<IEnumerable<TagGroupDto>> GetByMainTagGroupId(int id);
        Task<TagGroupDto?> GetByStringCode(string input);
    }
}
