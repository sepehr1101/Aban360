using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
 using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/assessment")]
    public class SetAssessmentResultController : BaseController
    {
        private readonly ISetAssessmentResultHandler _setAssessmentResultHandler;
        public SetAssessmentResultController(ISetAssessmentResultHandler setAssessmentResultHandler)
        {
            _setAssessmentResultHandler = setAssessmentResultHandler;
            _setAssessmentResultHandler.NotNull(nameof(setAssessmentResultHandler));
        }

        [HttpPost]
        [Route("result")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AssessmentResultInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetReult([FromBody] AssessmentResultInputDto inputDto, CancellationToken cancellationToken)
        {
            await _setAssessmentResultHandler.Handle(inputDto, cancellationToken);
            return Ok(inputDto);
        }
    }
}
