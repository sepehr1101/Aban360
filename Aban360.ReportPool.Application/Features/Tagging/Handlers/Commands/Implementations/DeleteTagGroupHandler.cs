using Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Contracts;
using Aban360.ReportPool.Persistence.Features.Tagging;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Implementations
{
    public sealed class DeleteTagGroupHandler : IDeleteTagGroupHandler
    {
        private readonly ITagGroupService _service;

        public DeleteTagGroupHandler(ITagGroupService service)
        {
            _service = service;
        }

        // Soft delete
        public async Task<bool> Handle(int id)
        {
            return await _service.Delete(id);
        }
    }
}