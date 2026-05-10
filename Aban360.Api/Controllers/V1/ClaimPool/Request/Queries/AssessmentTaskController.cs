using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Queries
{
    [Route("v1/assessment")]
    public class AssessmentTaskController : BaseController
    {
        private readonly IAssessmentTaskGetAllHandler _assessmentTaskHandler;
        private readonly IAssessmentLocatoinsGetHandler _assessmentLocatoinsGetHandler;
        public AssessmentTaskController(
            IAssessmentTaskGetAllHandler assessmentTaskHandler,
            IAssessmentLocatoinsGetHandler assessmentLocatoinsGetHandler)
        {
            _assessmentTaskHandler = assessmentTaskHandler;
            _assessmentTaskHandler.NotNull(nameof(assessmentTaskHandler));

            _assessmentLocatoinsGetHandler = assessmentLocatoinsGetHandler;
            _assessmentLocatoinsGetHandler.NotNull(nameof(assessmentLocatoinsGetHandler));
        }

        [HttpPost, HttpGet]
        [Route("task")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AssessmentTasksOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            int examinerCode = UserService.GetUserCode(CurrentUser.Username);
            AssessmentTasksOutputDto result = await _assessmentTaskHandler.Handle(examinerCode, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("locations/{trackId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AssessmentLocationsGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLocations(Guid trackId, CancellationToken cancellationToken)
        {
            AssessmentLocationsGetDto result = await _assessmentLocatoinsGetHandler.Handle(trackId, cancellationToken);
            return Ok(result);
        }
    }
}
