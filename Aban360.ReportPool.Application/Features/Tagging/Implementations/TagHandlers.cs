using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Tagging.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Persistence.Features.Tagging;

namespace Aban360.ReportPool.Application.Features.Tagging.Implementations
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

    // -------------------- READ --------------------
    public sealed class GetTagHandler : IGetTagHandler
    {
        private readonly ITagService _service;

        public GetTagHandler(ITagService service)
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
    public sealed class TagReportsHandler : ITagReportsHandler
    {
        private readonly ITagReportQueryService _service;
        public TagReportsHandler(ITagReportQueryService service)
        {
            _service = service;
            _service.NotNull(nameof(service));
        }
        public async Task<ReportOutput<TagsHeaderOutputDto, TagsReportSummaryDataOutputDto>> Handle(TagsInputDto inputDto, bool isZoneTitle, CancellationToken cancellationToken)
        {
            return await _service.Get(inputDto, isZoneTitle);
        }
    }

    // -------------------- UPDATE --------------------
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

    // -------------------- DELETE --------------------
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