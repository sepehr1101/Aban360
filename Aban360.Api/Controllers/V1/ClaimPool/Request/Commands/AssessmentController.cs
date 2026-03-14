using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
 using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/assessment")]
    public class AssessmentController : BaseController
    {
        private readonly ISetAssessmentResultHandler _setAssessmentResultHandler;
        private readonly ISetAssessmentTimeHandler _setAssessmentTimeHandler;
        public AssessmentController(
            ISetAssessmentResultHandler setAssessmentResultHandler,
            ISetAssessmentTimeHandler setAssessmentTimeHandler)
        {
            _setAssessmentResultHandler = setAssessmentResultHandler;
            _setAssessmentResultHandler.NotNull(nameof(setAssessmentResultHandler));
            
            _setAssessmentTimeHandler = setAssessmentTimeHandler;
            _setAssessmentTimeHandler.NotNull(nameof(setAssessmentTimeHandler));
        }

        [HttpPost]
        [Route("result")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AssessmentResultInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetReult([FromBody] AssessmentResultInputDto inputDto, CancellationToken cancellationToken)
        {
            int examinerCode = GetUserCode();
            await _setAssessmentResultHandler.Handle(inputDto, examinerCode, cancellationToken);
            return Ok(inputDto);
        }
        
        [HttpPost]
        [Route("set-time")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AssessmentResultInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetTime([FromBody] AssessmentSetTimeInputDto inputDto, CancellationToken cancellationToken)
        {
            int examinerCode = GetUserCode();
            await _setAssessmentTimeHandler.Handle(inputDto, examinerCode, cancellationToken);
            return Ok(inputDto);
        }

        private int GetUserCode()
        {
            bool isSuccess = int.TryParse(CurrentUser.Username, out int userCode);
            if (!isSuccess)
            {
                throw new InvalidBillIdException(ExceptionLiterals.InvalidUserName);
            }

            return userCode;
        }
    }
}
