using Aban360.ReportPool.Application.Features.Tagging.Handlers.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Persistence.Features.Tagging;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Queries.Implementations
{
    public sealed class MainTagGroupGetAllHandler : IMainTagGroupGetAllHandler
    {
        private readonly IMainTagGroupService _service;
        public MainTagGroupGetAllHandler(IMainTagGroupService service)
        {
            _service = service;
        }

        public async Task<IEnumerable<MainTagGroupGetDto>> Handle()
        {
            IEnumerable<MainTagGroupGetDto> data = await _service.GetValid();
            return data;
        }
    }
}
