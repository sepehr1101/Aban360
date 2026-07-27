using Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging.Commands;
using Aban360.ReportPool.Persistence.Features.Tagging.Queries.Contracts;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Implementations
{
    public sealed class MainTagGroupDeleteHandler : IMainTagGroupDeleteHandler
    {
        private readonly IMainTagGroupService _service;
        public MainTagGroupDeleteHandler(IMainTagGroupService service)
        {
            _service = service;
        }

        public async Task Handle(int id)
        {
            MainTagGroupRemoveDto removeDto = new(id);
            await _service.Remove(removeDto);
        }
    }
}
