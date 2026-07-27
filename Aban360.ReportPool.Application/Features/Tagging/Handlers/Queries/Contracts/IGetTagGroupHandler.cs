using Aban360.ReportPool.Domain.Features.Tagging;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Queries.Contracts
{
    public interface IGetTagGroupHandler
    {
        Task<TagGroupDto?> Handle(int id);
        Task<IEnumerable<TagGroupDto>> HandleAll();
    }
}