using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Tagging.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.Tagging.Queries
{
    [Route("v1/tag-summary")]
    public class TagSummaryController : BaseController
    {
        private readonly ITagReportsHandler _reportHandler;

        public TagSummaryController(ITagReportsHandler reportHandler)
        {
            _reportHandler = reportHandler;
            _reportHandler.NotNull(nameof(reportHandler));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<TagsHeaderOutputDto, TagsReportSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        [Route("by-zone/raw")]
        public async Task<IActionResult> RawByZone(TagsInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<TagsHeaderOutputDto, TagsReportSummaryDataOutputDto> result = await _reportHandler.Handle(inputDto, true, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<TagsHeaderOutputDto, TagsReportSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        [Route("by-usage/raw")]
        public async Task<IActionResult> RawByUsage(TagsInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<TagsHeaderOutputDto, TagsReportSummaryDataOutputDto> result = await _reportHandler.Handle(inputDto, false, cancellationToken);
            return Ok(result);
        }
    }
}
