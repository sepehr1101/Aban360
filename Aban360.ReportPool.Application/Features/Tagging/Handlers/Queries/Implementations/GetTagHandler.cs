using Aban360.ReportPool.Application.Features.Tagging.Handlers.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Persistence.Features.Tagging.Queries.Contracts;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Queries.Implementations
{
    public sealed class GetTagHandler : IGetTagHandler
    {
        private readonly ITagQueryService _service;

        public GetTagHandler(ITagQueryService service)
        {
            _service = service;
        }

        public async Task<TagDto?> Handle(int id)
        {
            return await _service.GetById(id);
        }

        public async Task<IEnumerable<TagDto>> HandleAll()
        {
            return await _service.GetAll();
        }
    }
}