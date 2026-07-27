using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Tagging.Handlers.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Persistence.Features.Tagging;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Queries.Implementations
{
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
}