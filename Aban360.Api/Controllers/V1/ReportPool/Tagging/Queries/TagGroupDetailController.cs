using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Tagging.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.Tagging.Queries
{
    [Route("v1/tag-group-detail")]
    public class TagGroupDetailController : BaseController
    {
        private readonly ITagGroupReportsHandler _reportHandler;

        public TagGroupDetailController(ITagGroupReportsHandler reportHandler)
        {
            _reportHandler = reportHandler;
            _reportHandler.NotNull(nameof(reportHandler));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<TagsHeaderOutputDto, TagGroupReportDetailDataOutputDto>>), StatusCodes.Status200OK)]
        [Route("raw")]
        public async Task<IActionResult> Raw(TagsInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<TagsHeaderOutputDto, TagGroupReportDetailDataOutputDto> result = await _reportHandler.Handle(inputDto,cancellationToken);
            return Ok(result);
        }
    }
}
