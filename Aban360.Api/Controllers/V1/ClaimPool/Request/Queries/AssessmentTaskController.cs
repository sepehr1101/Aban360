using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Queries
{
    [Route("v1/assessment")]
    public class AssessmentTaskController : BaseController
    {
        private readonly IAssessmentTaskGetAllHandler _assessmentTaskHandler;
        public AssessmentTaskController(IAssessmentTaskGetAllHandler assessmentTaskHandler)
        {
            _assessmentTaskHandler = assessmentTaskHandler;
            _assessmentTaskHandler.NotNull(nameof(assessmentTaskHandler));
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
    }
}
