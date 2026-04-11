using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/assessment")]
    public class AssessmentController : BaseController
    {
        private readonly ISetAssessmentResultHandler _setAssessmentResultHandler;
        private readonly ISetAssessmentTimeHandler _setAssessmentTimeHandler;
        private readonly ISetLightAssessmentResultHandler _setLightAssessmentResultHandler;
        private readonly IReAssessmentRequestHandler _reAssessmentRequestHandler;
        public AssessmentController(
            ISetAssessmentResultHandler setAssessmentResultHandler,
            ISetAssessmentTimeHandler setAssessmentTimeHandler,
            ISetLightAssessmentResultHandler setLightAssessmentResultHandler,
            IReAssessmentRequestHandler reAssessmentRequestHandler)
        {
            _setAssessmentResultHandler = setAssessmentResultHandler;
            _setAssessmentResultHandler.NotNull(nameof(setAssessmentResultHandler));

            _setAssessmentTimeHandler = setAssessmentTimeHandler;
            _setAssessmentTimeHandler.NotNull(nameof(setAssessmentTimeHandler));

            _setLightAssessmentResultHandler = setLightAssessmentResultHandler;
            _setLightAssessmentResultHandler.NotNull(nameof(setLightAssessmentResultHandler));

            _reAssessmentRequestHandler = reAssessmentRequestHandler;
            _reAssessmentRequestHandler.NotNull(nameof(reAssessmentRequestHandler));
        }

        [HttpPost]
        [Route("result")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AssessmentResultInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetReult([FromBody] AssessmentResultInputDto inputDto, CancellationToken cancellationToken)
        {
            int examinerCode = UserService.GetUserCode(CurrentUser.Username);
            await _setAssessmentResultHandler.Handle(inputDto, examinerCode, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("set-time")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AssessmentResultInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetTime([FromBody] AssessmentSetTimeInputDto inputDto, CancellationToken cancellationToken)
        {
            int examinerCode = UserService.GetUserCode(CurrentUser.Username);
            await _setAssessmentTimeHandler.Handle(inputDto, examinerCode, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("set-light-result")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MoshtrakUpdateInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetAssessmentLite([FromBody] LightAssessmentResultInputDto inputDto, CancellationToken cancellationToken)
        {
            int userName = UserService.GetUserCode(CurrentUser.Username);
            await _setLightAssessmentResultHandler.Handle(inputDto, userName, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("reAssessment")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TrackNumberWithDescriptionInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ReAssessment([FromBody] TrackNumberWithDescriptionInputDto inputDto, CancellationToken cancellationToken)
        {
            int userName = UserService.GetUserCode(CurrentUser.Username);
            await _reAssessmentRequestHandler.Handle(inputDto, userName, cancellationToken);
            return Ok(inputDto);
        }
    }
}
