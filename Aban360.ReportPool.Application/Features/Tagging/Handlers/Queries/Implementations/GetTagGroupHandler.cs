using Aban360.ReportPool.Application.Features.Tagging.Handlers.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Persistence.Features.Tagging.Queries.Contracts;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Queries.Implementations
{
    public sealed class GetTagGroupHandler : IGetTagGroupHandler
    {
        private readonly ITagGroupQueryService _service;

        public GetTagGroupHandler(ITagGroupQueryService service)
        {
            _service = service;
        }

        public async Task<TagGroupDto?> Handle(int id)
        {
            return await _service.GetById(id);
        }

        public async Task<IEnumerable<TagGroupDto>> HandleAll()
        {
            return await _service.GetAll();
        }
    }
}