using Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Contracts;
using Aban360.ReportPool.Persistence.Features.Tagging;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Implementations
{
    public sealed class DeleteTagHandler : IDeleteTagHandler
    {
        private readonly ITagService _service;

        public DeleteTagHandler(ITagService service)
        {
            _service = service;
        }

        public async Task<bool> Handle(int id)
        {
            return await _service.Delete(id);
        }
    }
}