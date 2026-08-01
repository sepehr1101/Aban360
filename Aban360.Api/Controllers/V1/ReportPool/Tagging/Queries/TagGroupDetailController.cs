using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Tagging.Handlers.Queries.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.Tagging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.Tagging.Queries
{
    [Route("v1/tag-group-detail")]
    public class TagGroupDetailController : BaseController
    {
        private readonly ITagGroupReportsHandler _reportHandler;
        private readonly IReportGenerator _reportGenerator;
        public TagGroupDetailController(
            ITagGroupReportsHandler reportHandler,
            IReportGenerator reportGenerator)
        {
            _reportHandler = reportHandler;
            _reportHandler.NotNull(nameof(reportHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(reportGenerator));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<TagsHeaderOutputDto, TagGroupReportDetailDataOutputDto>>), StatusCodes.Status200OK)]
        [Route("raw")]
        public async Task<IActionResult> Raw(TagsInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<TagsHeaderOutputDto, TagGroupReportDetailDataOutputDto> result = await _reportHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, TagsInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _reportHandler.Handle, CurrentUser, ReportLiterals.TagGroupDetail, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(TagsInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 640;
            ReportOutput<TagsHeaderOutputDto, TagGroupReportDetailDataOutputDto> result = await _reportHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
