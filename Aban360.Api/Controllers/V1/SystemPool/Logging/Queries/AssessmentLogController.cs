using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.SystemPool.Application.Features.Logging.Handlers.Queries.Conracts;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Input;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Output;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Aban360.SystemPool.Application.Features.Logging.Services;

namespace Aban360.Api.Controllers.V1.SystemPool.Logging.Queries
{
    [Route("v1/assessment-log")]
    public class AssessmentLogController : BaseController
    {
        private readonly IAssessmentLogService _assessmentLogService;
        private string _folderPath = @"AppData\AssessmentLogs";
        public AssessmentLogController(IAssessmentLogService assessmentLogService)
        {
            _assessmentLogService = assessmentLogService;
            _assessmentLogService.NotNull(nameof(assessmentLogService));
        }

        [Route("upload")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AssessmentLogInsertDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UploadFile(AssessmentLogInsertDto inputDto, CancellationToken cancellation)
        {
            await _assessmentLogService.SaveAsync(inputDto, cancellation);
            return Ok(inputDto);
        }

        [Route("get")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<AssessmentLogGetDto>>), StatusCodes.Status200OK)]
        public IActionResult GetAll(CancellationToken cancellation)
        {
            IEnumerable<AssessmentLogGetDto> result = _assessmentLogService.GetAll(cancellation);
            return Ok(result);
        }

        [Route("get/{fileName}")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AssessmentLogFileGetDto>), StatusCodes.Status200OK)]
        public FileResult Get(string fileName, CancellationToken cancellation)
        {
            string fullPath = Path.Combine(_folderPath, fileName);
            if (!System.IO.File.Exists(fullPath))
            {
                throw new InvalidTrackingException(ExceptionLiterals.NotFoundFile);
            }

            FileStream fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return File(fileStream, "text/plain", fileName, enableRangeProcessing: true);
        //AssessmentLogFileGetDto result = _assessmentLogGetByFileNameHandler.Handle(fileName, cancellation);
        //return Ok(result);
        }
    }
}
