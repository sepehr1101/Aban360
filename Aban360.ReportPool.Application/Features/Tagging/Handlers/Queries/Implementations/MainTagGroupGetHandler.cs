using Aban360.ReportPool.Application.Features.Tagging.Handlers.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Persistence.Features.Tagging.Queries.Contracts;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Queries.Implementations
{
    public sealed class MainTagGroupGetHandler : IMainTagGroupGetHandler
    {
        private readonly IMainTagGroupQueryService _service;
        public MainTagGroupGetHandler(IMainTagGroupQueryService service)
        {
            _service = service;
        }

        public async Task<MainTagGroupGetDto> Handle(int id)
        {
            MainTagGroupGetDto data = await _service.GetValid(id);
            return data;
        }
    }
}
