using Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging.Commands;
using Aban360.ReportPool.Persistence.Features.Tagging.Queries.Contracts;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Implementations
{
    public sealed class MainTagGroupUpdateHandler : IMainTagGroupUpdateHandler
    {
        private readonly IMainTagGroupService _service;
        public MainTagGroupUpdateHandler(IMainTagGroupService service)
        {
            _service = service;
        }

        public async Task Handle(MainTagGroupUpdateDto input)
        {
            await _service.Update(input);
        }
    }
}
