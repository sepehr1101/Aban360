using Aban360.ReportPool.Application.Features.Tagging.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Persistence.Features.Tagging;

namespace Aban360.ReportPool.Application.Features.Tagging.Implementations
{
    public sealed class MainTagGroupGetHandler : IMainTagGroupGetHandler
    {
        private readonly IMainTagGroupService _service;
        public MainTagGroupGetHandler(IMainTagGroupService service)
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
