using Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging.Commands;
using Aban360.ReportPool.Persistence.Features.Tagging.Queries.Contracts;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Implementations
{
    public sealed class UpdateTagHandler : IUpdateTagHandler
    {
        private readonly ITagService _service;

        public UpdateTagHandler(ITagService service)
        {
            _service = service;
        }

        public async Task<bool> Handle(UpdateTagDto dto)
        {
            return await _service.Update(dto);
        }
    }
}