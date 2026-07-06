using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.SystemPool.Application.Features.Logging.Handlers.Commands.Contracts;
using Aban360.SystemPool.Application.Features.Logging.Handlers.Queries.Conracts;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Input;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Output;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.SystemPool.Logging.Queries
{
    [Route("v1/assessment-log")]
    public class AssessmentLogController : BaseController
    {
        private readonly IAssessmentLogSaveHandler _assessmentLogSaveHandler;
        private readonly IAssessmentLogGetAllHandler _assessmentGetAllHandler;
        private readonly IAssessmentLogGetByFileNameHandler _assessmentLogGetByFileNameHandler;
        public AssessmentLogController(
            IAssessmentLogSaveHandler assessmentLogSaveHandler,
            IAssessmentLogGetAllHandler assessmentGetAllHandler,
            IAssessmentLogGetByFileNameHandler assessmentLogGetByFileNameHandler)
        {
            _assessmentLogSaveHandler = assessmentLogSaveHandler;
            _assessmentLogSaveHandler.NotNull(nameof(assessmentLogSaveHandler));

            _assessmentGetAllHandler = assessmentGetAllHandler;
            _assessmentGetAllHandler.NotNull(nameof(assessmentGetAllHandler));

            _assessmentLogGetByFileNameHandler = assessmentLogGetByFileNameHandler;
            _assessmentLogGetByFileNameHandler.NotNull(nameof(assessmentLogGetByFileNameHandler));
        }

        [Route("upload")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AssessmentLogInsertDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UploadFile(AssessmentLogInsertDto inputDto, CancellationToken cancellation)
        {
            await _assessmentLogSaveHandler.Handle(inputDto, cancellation);
            return Ok(inputDto);
        }

        [Route("get")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<AssessmentLogGetDto>>), StatusCodes.Status200OK)]
        public IActionResult GetAll(CancellationToken cancellation)
        {
            IEnumerable<AssessmentLogGetDto> result = _assessmentGetAllHandler.Handle(cancellation);
            return Ok(result);
        }

        [Route("get/{fileName}")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AssessmentLogFileGetDto>), StatusCodes.Status200OK)]
        public IActionResult Get(string fileName, CancellationToken cancellation)
        {
            AssessmentLogFileGetDto result = _assessmentLogGetByFileNameHandler.Handle(fileName, cancellation);
            return Ok(result);
        }
    }
}
