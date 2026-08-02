using Aban360.ReportPool.Application.Features.Tagging.Handlers.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Persistence.Features.Tagging.Queries.Contracts;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Queries.Implementations
{
    public sealed class MainTagGroupGetAllHandler : IMainTagGroupGetAllHandler
    {
        private readonly IMainTagGroupQueryService _service;
        public MainTagGroupGetAllHandler(IMainTagGroupQueryService service)
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
