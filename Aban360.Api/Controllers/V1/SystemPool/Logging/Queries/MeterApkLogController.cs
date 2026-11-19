using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.SystemPool.Application.Features.Logging.Handlers.Queries.Conracts;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Input;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Output;
using Microsoft.AspNetCore.Mvc;
using Aban360.SystemPool.Application.Features.Logging.Services;

namespace Aban360.Api.Controllers.V1.SystemPool.Logging.Queries
{
    [Route("v1/meter-apk-log")]
    public class MeterApkLogController : BaseController
    {
        private readonly IMeterApkLogService _meterApkLogService;
        private string _folderPath = DirectoryLiterals.MeterApkLogsPath;
        public MeterApkLogController(IMeterApkLogService meterApkLogService)
        {
            _meterApkLogService = meterApkLogService;
            _meterApkLogService.NotNull(nameof(meterApkLogService));
        }

        [Route("upload")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterApkLogInsertDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UploadFile(MeterApkLogInsertDto inputDto, CancellationToken cancellation)
        {
            await _meterApkLogService.SaveAsync(inputDto, cancellation);
            return Ok(inputDto);
        }

        [Route("get")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<MeterApkLogGetDto>>), StatusCodes.Status200OK)]
        public IActionResult GetAll(CancellationToken cancellation)
        {
            IEnumerable<MeterApkLogGetDto> result = _meterApkLogService.GetAll(cancellation);
            return Ok(result);
        }

        [Route("get/{fileName}")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterApkLogFileGetDto>), StatusCodes.Status200OK)]
        public FileResult Get(string fileName, CancellationToken cancellation)
        {
            string fullPath = Path.Combine(_folderPath, fileName);
            if (!System.IO.File.Exists(fullPath))
            {
                throw new InvalidTrackingException(ExceptionLiterals.NotFoundFile);
            }

            FileStream fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return File(fileStream, "text/plain", fileName, enableRangeProcessing: true);
        }
    }
}
