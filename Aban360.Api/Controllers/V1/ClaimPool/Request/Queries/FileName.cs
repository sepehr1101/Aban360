using Aban360.Api.Cronjobs;
using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Queries
{
    [Route("v1/assessment")]
    public class AssessmentDuplicateController : BaseController
    {
        private readonly IAssessmentDuplicateDetailHandler _assessmentDuplicateDetailHandler;
        private readonly IReportGenerator _reportGenerator;
        public AssessmentDuplicateController(
            IAssessmentDuplicateDetailHandler assessmentDuplicateDetailHandler,
            IReportGenerator reportGenerator)
        {
            _assessmentDuplicateDetailHandler = assessmentDuplicateDetailHandler;
            _assessmentDuplicateDetailHandler.NotNull(nameof(assessmentDuplicateDetailHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost]
        [Route("duplicate-raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<AssessmentDuplicatHeaderOutputDto, AssessmentDuplicateDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw([FromBody] AssessmentDuplicateInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<AssessmentDuplicatHeaderOutputDto, AssessmentDuplicateDetailDataOutputDto> result = await _assessmentDuplicateDetailHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("duplicate-excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, AssessmentDuplicateInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _assessmentDuplicateDetailHandler.Handle, CurrentUser, ReportLiterals.ConsumptionManagerDetail, connectionId);
            return Ok(inputDto);
        }
    }
}
