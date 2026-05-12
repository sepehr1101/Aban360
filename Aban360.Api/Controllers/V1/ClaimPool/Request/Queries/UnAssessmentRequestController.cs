using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Queries
{
    [Route("v1/request")]
    public class UnAssessmentRequestController : BaseController
    {
        private readonly IUnAssessmentGetAllHandler _unAssessmentHandler;
        public UnAssessmentRequestController(IUnAssessmentGetAllHandler unAssessmentHandler)
        {
            _unAssessmentHandler = unAssessmentHandler;
            _unAssessmentHandler.NotNull(nameof(unAssessmentHandler));
        }

        [HttpPost, HttpGet]
        [Route("unassessment")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<UnAssessmentHeaderOutputDto, UnAssessmentDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUnAssessment(CancellationToken cancellationToken)
        {
            ReportOutput<UnAssessmentHeaderOutputDto, UnAssessmentDataOutputDto> result = await _unAssessmentHandler.Handle(CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
