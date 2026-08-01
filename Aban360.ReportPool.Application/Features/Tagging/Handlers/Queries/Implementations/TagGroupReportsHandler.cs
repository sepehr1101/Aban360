using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Tagging.Handlers.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Persistence.Features.Tagging.Queries.Contracts;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Queries.Implementations
{
    public sealed class TagGroupReportsHandler : ITagGroupReportsHandler
    {
        private readonly ITagGroupReportQueryService _service;
        public TagGroupReportsHandler(ITagGroupReportQueryService service)
        {
            _service = service;
            _service.NotNull(nameof(service));
        }

        public async Task<ReportOutput<TagsHeaderOutputDto, TagGroupReportDetailDataOutputDto>> Handle(TagsInputDto inputDto, CancellationToken cancellationToken)
        {
            return await _service.Get(inputDto);
        }

        public async Task<ReportOutput<TagsHeaderOutputDto, TagsReportSummaryDataOutputDto>> SummaryHandle(TagsInputDto inputDto, bool isZoneTitle, CancellationToken cancellationToken)
        {
            return await _service.Get(inputDto, isZoneTitle);
        }
    }
}