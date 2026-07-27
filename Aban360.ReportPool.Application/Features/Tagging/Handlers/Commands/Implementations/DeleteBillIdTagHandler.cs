using Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Contracts;
using Aban360.ReportPool.Persistence.Features.Tagging;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Implementations
{
    internal sealed class DeleteBillIdTagHandler : IDeleteBillIdTagHandler
    {
        private readonly IBillIdTagService _service;
        public DeleteBillIdTagHandler(IBillIdTagService service) => _service = service;
        public async Task<bool> Handle(long id) => await _service.Delete(id);
    }
}
