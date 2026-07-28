using Aban360.ReportPool.Domain.Features.Tagging;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Queries.Contracts
{
    public interface IMainTagGroupGetAllHandler
    {
        Task<IEnumerable<MainTagGroupGetDto>> Handle();
    }
}
