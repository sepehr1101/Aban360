using Aban360.ReportPool.Domain.Features.Tagging;

namespace Aban360.ReportPool.Application.Features.Tagging.Contracts
{
    public interface IGetTagHandler
    {
        Task<TagDto?> Handle(int id);
        Task<IEnumerable<TagDto>> HandleAll();
    }
}