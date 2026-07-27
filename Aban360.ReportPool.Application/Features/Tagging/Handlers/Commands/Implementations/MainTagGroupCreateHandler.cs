using Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Persistence.Features.Tagging;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Implementations
{
    public sealed class MainTagGroupCreateHandler : IMainTagGroupCreateHandler
    {
        private readonly IMainTagGroupService _service;
        public MainTagGroupCreateHandler(IMainTagGroupService service)
        {
            _service = service;
        }

        public async Task Handle(MainTagGroupInsertInputDto input)
        {
            MainTagGroupInsertDto insertDto = new(input.Title);
            await _service.Insert(insertDto);
        }
    }
}
