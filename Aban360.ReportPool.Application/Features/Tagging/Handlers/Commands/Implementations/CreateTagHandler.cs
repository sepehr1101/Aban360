using Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Persistence.Features.Tagging;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Implementations
{
    public sealed class CreateTagHandler : ICreateTagHandler
    {
        private readonly ITagService _service;

        public CreateTagHandler(ITagService service)
        {
            _service = service;
        }

        public async Task<int> Handle(CreateTagDto dto)
        {
            return await _service.Create(dto);
        }
    }
}